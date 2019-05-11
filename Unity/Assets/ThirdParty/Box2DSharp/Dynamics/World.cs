using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Dynamics.Internal;
using Box2DSharp.Dynamics.Joints;

namespace Box2DSharp.Dynamics
{
    public class World : IDisposable
    {
        /// <summary>
        /// This is used to compute the time step ratio to
        /// support a variable time step.
        /// 时间步倍率
        /// </summary>
        private float _invDt0;

        /// <summary>
        /// 时间步完成
        /// </summary>
        private bool _stepComplete;

        /// <summary>
        /// 新增夹具
        /// </summary>
        private bool _hasNewFixture;

        /// <summary>
        /// Register a destruction listener. The listener is owned by you and must
        /// remain in scope.
        /// 析构监听器
        /// </summary>
        public IDestructionListener DestructionListener { get; set; }

        /// <summary>
        /// 是否启用连续碰撞
        /// </summary>
        public bool ContinuousPhysics { get; set; }

        /// <summary>
        /// 重力常数
        /// </summary>
        public Vector2 Gravity { get; set; }

        /// <summary>
        /// 清除受力
        /// </summary>
        public bool IsAutoClearForces { get; set; }

        /// <summary>
        /// 锁定世界
        /// </summary>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// 世界是否允许休眠
        /// </summary>
        public bool AllowSleep
        {
            get => _allowSleep;
            set
            {
                if (_allowSleep == value)
                {
                    return;
                }

                _allowSleep = value;
                if (_allowSleep == false)
                {
                    var node = BodyList.First;
                    while (node != null)
                    {
                        node.Value.IsAwake = true;
                        node = node.Next;
                    }
                }
            }
        }

        private bool _allowSleep;

        /// <summary>
        /// Enable/disable single stepped continuous physics. For testing. 
        /// 子步进
        /// </summary>
        public bool SubStepping { get; set; }

        /// <summary>
        /// These are for debugging the solver.
        /// Enable/disable warm starting. For testing.
        /// 热启动,用于调试求解器
        /// </summary>
        public bool WarmStarting { get; set; }

        /// <summary>
        /// 性能统计
        /// </summary>
        public Profile Profile;

        public ToiProfile ToiProfile { get; set; } = null;

        public GJkProfile GJkProfile { get; set; } = null;

        /// <summary>
        /// 接触点管理器
        /// </summary>
        public ContactManager ContactManager { get; } = new ContactManager();

        /// <summary>
        /// 物体链表
        /// </summary>
        public LinkedList<Body> BodyList { get; } = new LinkedList<Body>();

        /// <summary>
        /// 关节链表
        /// </summary>
        public LinkedList<Joint> JointList { get; } = new LinkedList<Joint>();

        /// Get the number of broad-phase proxies.
        public int ProxyCount => ContactManager.BroadPhase.GetProxyCount();

        /// Get the number of bodies.
        public int BodyCount => BodyList.Count;

        /// Get the number of joints.
        public int JointCount => JointList.Count;

        /// Get the number of contacts (each may have 0 or more contact points).
        public int ContactCount => ContactManager.ContactList.Count;

        /// Get the height of the dynamic tree.
        public int TreeHeight => ContactManager.BroadPhase.GetTreeHeight();

        /// Get the balance of the dynamic tree.
        public int TreeBalance => ContactManager.BroadPhase.GetTreeBalance();

        /// Get the quality metric of the dynamic tree. The smaller the better.
        /// The minimum is 1.
        public float TreeQuality => ContactManager.BroadPhase.GetTreeQuality();

        public bool Disposed { get; private set; }

        public World() : this(new Vector2(0, -10))
        { }

        public World(in Vector2 gravity)
        {
            Gravity = gravity;

            WarmStarting = true;
            ContinuousPhysics = true;
            SubStepping = false;
            _stepComplete = true;

            AllowSleep = true;
            IsAutoClearForces = true;
            _invDt0 = 0.0f;
            Profile = new Profile();
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
            ContactManager.Dispose();
        }

        internal void NotifyNewFixture()
        {
            _hasNewFixture = true;
        }

        private void ResetNewFixture()
        {
            _hasNewFixture = false;
        }

        /// <summary>
        /// Register a contact filter to provide specific control over collision.
        /// Otherwise the default filter is used (b2_defaultFilter). The listener is
        /// owned by you and must remain in scope.
        /// 注册碰撞过滤器,用于在碰撞过程中执行自定义过滤
        /// </summary>
        /// <param name="filter"></param>
        public void SetContactFilter(IContactFilter filter)
        {
            ContactManager.ContactFilter = filter;
        }

        /// <summary>
        /// Register a contact event listener. The listener is owned by you and must
        /// remain in scope.
        /// 注册接触监听器
        /// </summary>
        /// <param name="listener"></param>
        public void SetContactListener(IContactListener listener)
        {
            ContactManager.ContactListener = listener;
        }

        /// <summary>
        /// Create a rigid body given a definition. No reference to the definition
        /// is retained.
        /// @warning This function is locked during callbacks.
        /// 创建一个物体(刚体)
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public Body CreateBody(in BodyDef def)
        {
            Debug.Assert(IsLocked == false);
            if (IsLocked) // 世界锁定时无法创建物体
            {
                return null;
            }

            // 创建物体并关联到本世界
            var body = new Body(def, this);

            // Add to world doubly linked list.
            // 添加物体到物体链表头部
            body.Node = BodyList.AddFirst(body);
            return body;
        }

        /// <summary>
        /// Destroy a rigid body given a definition. No reference to the definition
        /// is retained. This function is locked during callbacks.
        /// @warning This automatically deletes all associated shapes and joints.
        /// @warning This function is locked during callbacks.
        /// 删除一个物体(刚体)
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool DestroyBody(Body body)
        {
            Debug.Assert(BodyList.Count > 0);
            Debug.Assert(IsLocked == false);
            if (IsLocked)
            {
                return false;
            }

            // Delete the attached joints.
            // 删除所有挂载的关节
            var jointEdgePointer = body.JointEdges.First;
            while (jointEdgePointer != default)
            {
                var next = jointEdgePointer.Next;
                DestructionListener?.SayGoodbye(jointEdgePointer.Value.Joint);
                DestroyJoint(jointEdgePointer.Value.Joint);
                jointEdgePointer = next;
            }

            // Delete the attached contacts.
            // 删除所有挂载的接触点
            var contactEdge = body.ContactEdges.First;
            while (contactEdge != default)
            {
                var next = contactEdge.Next;
                ContactManager.Destroy(contactEdge.Value.Contact);
                contactEdge = next;
            }

            // Delete the attached fixtures. This destroys broad-phase proxies.
            // 删除所有挂载的夹具,同时会删除对应的粗检测代理
            foreach (var fixture in body.Fixtures)
            {
                DestructionListener?.SayGoodbye(fixture);
                fixture.DestroyProxies(ContactManager.BroadPhase);
            }

            // Remove world body list.
            BodyList.Remove(body.Node);
            body.Dispose();
            return true;
        }

        /// <summary>
        /// Create a joint to constrain bodies together. No reference to the definition
        /// is retained. This may cause the connected bodies to cease colliding.
        /// @warning This function is locked during callbacks.
        /// 创建关节,用于把两个物体连接在一起,在回调中不可调用
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public Joint CreateJoint(JointDef def)
        {
            Debug.Assert(IsLocked == false);
            if (IsLocked)
            {
                return null;
            }

            var j = Joint.Create(def);

            // Connect to the world list.
            // 添加到关节列表头部
            j.Node = JointList.AddFirst(j);

            // Connect to the bodies' doubly linked lists.
            // 连接到物体的双向链表中
            j.EdgeA.Joint = j;
            j.EdgeA.Other = j.BodyB;
            j.EdgeA.Node = j.BodyA.JointEdges.AddFirst(j.EdgeA);

            j.EdgeB.Joint = j;
            j.EdgeB.Other = j.BodyA;
            j.EdgeB.Node = j.BodyB.JointEdges.AddFirst(j.EdgeB);

            var bodyA = def.BodyA;
            var bodyB = def.BodyB;

            // If the joint prevents collisions, then flag any contacts for filtering.
            if (def.CollideConnected == false)
            {
                var node = bodyB.ContactEdges.First;
                while (node != null)
                {
                    var contactEdge = node.Value;
                    node = node.Next;
                    if (contactEdge.Other == bodyA)
                    {
                        // Flag the contact for filtering at the next time step (where either
                        // body is awake).
                        contactEdge.Contact.FlagForFiltering();
                    }
                }
            }

            // Note: creating a joint doesn't wake the bodies.

            return j;
        }

        /// Destroy a joint. This may cause the connected bodies to begin colliding.
        /// @warning This function is locked during callbacks.
        public void DestroyJoint(Joint joint)
        {
            Debug.Assert(IsLocked == false);
            if (IsLocked)
            {
                return;
            }

            var collideConnected = joint.CollideConnected;

            // Remove from the doubly linked list.
            JointList.Remove(joint.Node);

            // Disconnect from island graph.
            // Wake up connected bodies.
            var bodyA = joint.BodyA;
            bodyA.IsAwake = true;
            bodyA.JointEdges.Remove(joint.EdgeA.Node);

            var bodyB = joint.BodyB;
            bodyB.IsAwake = true;
            bodyB.JointEdges.Remove(joint.EdgeB.Node);

            Debug.Assert(JointList.Count > 0);

            // If the joint prevents collisions, then flag any contacts for filtering.
            if (collideConnected == false)
            {
                var node = bodyB.ContactEdges.First;
                while (node != null)
                {
                    var contactEdge = node.Value;
                    node = node.Next;
                    if (contactEdge.Other == bodyA)
                    {
                        // Flag the contact for filtering at the next time step (where either
                        // body is awake).
                        contactEdge.Contact.FlagForFiltering();
                    }
                }
            }
        }

        private readonly Stopwatch _stepTimer = new Stopwatch();

        private readonly Stopwatch _timer = new Stopwatch();

        /// <summary>
        /// Take a time step. This performs collision detection, integration, and constraint solution.
        /// </summary>
        /// <param name="timeStep">the amount of time to simulate, this should not vary.</param>
        /// <param name="velocityIterations">for the velocity constraint solver.</param>
        /// <param name="positionIterations">for the position constraint solver.</param>
        public void Step(float timeStep, int velocityIterations, int positionIterations)
        {
            // profile 计时
            _stepTimer.Restart();

            // If new fixtures were added, we need to find the new contacts.
            // 如果存在新增夹具,则需要找到新接触点
            if (_hasNewFixture)
            {
                // 寻找新接触点
                ContactManager.FindNewContacts();

                // 去除新增夹具标志
                ResetNewFixture();
            }

            // 锁定世界
            IsLocked = true;

            // 时间间隔与迭代次数
            var step = new TimeStep
            {
                Dt = timeStep, VelocityIterations = velocityIterations,
                PositionIterations = positionIterations
            };

            // 计算时间间隔倒数
            if (timeStep > 0.0f)
            {
                step.InvDt = 1.0f / timeStep;
            }
            else
            {
                step.InvDt = 0.0f;
            }

            step.DtRatio = _invDt0 * timeStep;

            step.WarmStarting = WarmStarting;
            _timer.Restart();

            // Update contacts. This is where some contacts are destroyed.
            // 更新接触点
            {
                ContactManager.Collide();
                _timer.Stop();
                Profile.Collide = _timer.ElapsedMilliseconds;
            }

            // Integrate velocities, solve velocity constraints, and integrate positions.
            // 对速度进行积分，求解速度约束，整合位置
            if (_stepComplete && step.Dt > 0.0f)
            {
                _timer.Restart();
                Solve(step);
                _timer.Stop();
                Profile.Solve = _timer.ElapsedMilliseconds;
            }

            // Handle TOI events.
            // 处理碰撞时间
            if (ContinuousPhysics && step.Dt > 0.0f)
            {
                _timer.Restart();
                SolveTOI(step);
                _timer.Stop();
                Profile.SolveTOI = _timer.ElapsedMilliseconds;
            }

            if (step.Dt > 0.0f)
            {
                _invDt0 = step.InvDt;
            }

            // 启用受力清理
            if (IsAutoClearForces)
            {
                ClearForces();
            }

            // 时间步完成,解锁世界
            IsLocked = false;
            _stepTimer.Stop();
            Profile.Step = _stepTimer.ElapsedMilliseconds;
        }

        /// Manually clear the force buffer on all bodies. By default, forces are cleared automatically
        /// after each call to Step. The default behavior is modified by calling SetAutoClearForces.
        /// The purpose of this function is to support sub-stepping. Sub-stepping is often used to maintain
        /// a fixed sized time step under a variable frame-rate.
        /// When you perform sub-stepping you will disable auto clearing of forces and instead call
        /// ClearForces after all sub-steps are complete in one pass of your game loop.
        /// @see SetAutoClearForces
        public void ClearForces()
        {
            var node = BodyList.First;
            while (node != null)
            {
                var body = node.Value;
                node = node.Next;
                body.Force.SetZero();
                body.Torque = 0.0f;
            }
        }

        private class TreeQueryCallback : ITreeQueryCallback
        {
            public ContactManager ContactManager { get; private set; }

            public IQueryCallback Callback { get; private set; }

            public void Set(ContactManager contactManager, in IQueryCallback callback)
            {
                ContactManager = contactManager;
                Callback = callback;
            }

            public void Reset()
            {
                ContactManager = default;
                Callback = default;
            }

            /// <inheritdoc />
            public bool QueryCallback(int proxyId)
            {
                var proxy = (FixtureProxy) ContactManager
                                          .BroadPhase.GetUserData(proxyId);
                return Callback.QueryCallback(proxy.Fixture);
            }
        }

        /// Query the world for all fixtures that potentially overlap the
        /// provided AABB.
        /// @param callback a user implemented callback class.
        /// @param aabb the query box.
        public void QueryAABB(in IQueryCallback callback, in AABB aabb)
        {
            var cb = SimpleObjectPool<TreeQueryCallback>.Shared.Get();
            cb.Set(ContactManager, in callback);
            ContactManager.BroadPhase.Query(cb, aabb);
            cb.Reset();
            SimpleObjectPool<TreeQueryCallback>.Shared.Return(cb);
        }

        private class InternalRayCastCallback : ITreeRayCastCallback
        {
            public ContactManager ContactManager { get; private set; }

            public IRayCastCallback Callback { get; private set; }

            public void Set(ContactManager contactManager, in IRayCastCallback callback)
            {
                ContactManager = contactManager;
                Callback = callback;
            }

            public void Reset()
            {
                ContactManager = default;
                Callback = default;
            }

            public float RayCastCallback(in RayCastInput input, int proxyId)
            {
                var userData = ContactManager.BroadPhase.GetUserData(proxyId);
                var proxy = (FixtureProxy) userData;
                var fixture = proxy.Fixture;
                var index = proxy.ChildIndex;

                var hit = fixture.RayCast(out var output, input, index);

                if (!hit)
                {
                    return input.MaxFraction;
                }

                var fraction = output.Fraction;
                var point = (1.0f - fraction) * input.P1 + fraction * input.P2;
                return Callback.RayCastCallback(fixture, point, output.Normal, fraction);
            }
        }

        /// Ray-cast the world for all fixtures in the path of the ray. Your callback
        /// controls whether you get the closest point, any point, or n-points.
        /// The ray-cast ignores shapes that contain the starting point.
        /// @param callback a user implemented callback class.
        /// @param point1 the ray starting point
        /// @param point2 the ray ending point
        public void RayCast(in IRayCastCallback callback, in Vector2 point1, in Vector2 point2)
        {
            var input = new RayCastInput
            {
                MaxFraction = 1.0f, P1 = point1,
                P2 = point2
            };
            var cb = SimpleObjectPool<InternalRayCastCallback>.Shared.Get();
            cb.Set(ContactManager, in callback);
            ContactManager.BroadPhase.RayCast(cb, input);
            cb.Reset();
            SimpleObjectPool<InternalRayCastCallback>.Shared.Return(cb);
        }

        /// Shift the world origin. Useful for large worlds.
        /// The body shift formula is: position -= newOrigin
        /// @param newOrigin the new origin with respect to the old origin
        public void ShiftOrigin(in Vector2 newOrigin)
        {
            Debug.Assert(!IsLocked);
            if (IsLocked)
            {
                return;
            }

            var bodyNode = BodyList.First;
            while (bodyNode != null)
            {
                var b = bodyNode.Value;
                bodyNode = bodyNode.Next;
                b.Transform.Position -= newOrigin;
                b.Sweep.C0 -= newOrigin;
                b.Sweep.C -= newOrigin;
            }

            var jointNode = JointList.First;
            while (jointNode != null)
            {
                jointNode.Value.ShiftOrigin(newOrigin);
                jointNode = jointNode.Next;
            }

            ContactManager.BroadPhase.ShiftOrigin(newOrigin);
        }

        private readonly Stack<Body> _solveStack = new Stack<Body>(256);

        private readonly Stopwatch _solveTimer = new Stopwatch();

        /// <summary>
        /// Find islands, integrate and solve constraints, solve position constraints
        /// 找出岛屿,迭代求解约束,求解位置约束(岛屿用来对物理空间进行物体分组求解,提高效率)
        /// </summary>
        /// <param name="step"></param>
        private void Solve(in TimeStep step)
        {
            Profile.SolveInit = 0.0f;
            Profile.SolveVelocity = 0.0f;
            Profile.SolvePosition = 0.0f;

            // Size the island for the worst case.
            // 最坏情况岛屿容量,即全世界在同一个岛屿
            var island = new Island(
                BodyList.Count,
                ContactManager.ContactList.Count,
                JointList.Count,
                ContactManager.ContactListener);

            // Clear all the island flags.
            // 清除所有岛屿标志
            var bodyNode = BodyList.First;
            while (bodyNode != null)
            {
                bodyNode.Value.UnsetFlag(BodyFlags.Island);
                bodyNode = bodyNode.Next;
            }

            var contactNode = ContactManager.ContactList.First;
            while (contactNode != null)
            {
                contactNode.Value.Flags &= ~Contact.ContactFlag.IslandFlag;
                contactNode = contactNode.Next;
            }

            var jointNode = JointList.First;
            while (jointNode != null)
            {
                jointNode.Value.IslandFlag = false;
                jointNode = jointNode.Next;
            }

            // Build and simulate all awake islands.
            var stack = _solveStack;
            stack.Clear();

            bodyNode = BodyList.First;
            while (bodyNode != null)
            {
                var body = bodyNode.Value;
                bodyNode = bodyNode.Next;
                if (body.HasFlag(BodyFlags.Island)) // 已经分配到岛屿则跳过
                {
                    continue;
                }

                if (body.IsAwake == false || body.IsActive == false) // 跳过休眠物体
                {
                    continue;
                }

                // The seed can be dynamic or kinematic.
                if (body.BodyType == BodyType.StaticBody) // 跳过静态物体
                {
                    continue;
                }

                // Reset island and stack.
                island.Clear();

                //var stackCount = 0;
                stack.Push(body);

                //stackCount++;
                body.SetFlag(BodyFlags.Island);

                // Perform a depth first search (DFS) on the constraint graph.
                while (stack.Count > 0)
                {
                    // Grab the next body off the stack and add it to the island.
                    //--stackCount;
                    var b = stack.Pop();
                    Debug.Assert(b.IsActive);
                    island.Add(b);

                    // Make sure the body is awake (without resetting sleep timer).
                    b.SetFlag(BodyFlags.IsAwake);

                    // To keep islands as small as possible, we don't
                    // propagate islands across static bodies.
                    if (b.BodyType == BodyType.StaticBody)
                    {
                        continue;
                    }

                    // Search all contacts connected to this body.
                    // 查找该物体所有接触点
                    var node = b.ContactEdges.First;
                    while (node != null)
                    {
                        var contactEdge = node.Value;
                        node = node.Next;

                        var contact = contactEdge.Contact;

                        // Has this contact already been added to an island?
                        // 接触点已经标记岛屿,跳过
                        if (contact.HasFlag(Contact.ContactFlag.IslandFlag))
                        {
                            continue;
                        }

                        // Is this contact solid and touching?
                        // 接触点未启用或未接触,跳过
                        if (contact.IsEnabled == false || contact.IsTouching == false)
                        {
                            continue;
                        }

                        // Skip sensors.
                        // 跳过传感器
                        if (contact.FixtureA.IsSensor || contact.FixtureB.IsSensor)
                        {
                            continue;
                        }

                        // 将该接触点添加到岛屿中,并添加岛屿标志
                        island.Add(contact);
                        contact.Flags |= Contact.ContactFlag.IslandFlag;

                        var other = contactEdge.Other;

                        // Was the other body already added to this island?
                        // 如果接触边缘的另一个物体已经添加到岛屿则跳过
                        if (other.HasFlag(BodyFlags.Island))
                        {
                            continue;
                        }

                        // 否则将另一边的物体也添加到岛屿
                        //Debug.Assert(stackCount < stackSize);
                        stack.Push(other);
                        other.SetFlag(BodyFlags.Island);
                    }

                    // Search all joints connect to this body.
                    // 将该物体的关节所关联的物体也加入到岛屿中
                    var jointEdgeNode = b.JointEdges.First;
                    while (jointEdgeNode != null)
                    {
                        var je = jointEdgeNode.Value;
                        jointEdgeNode = jointEdgeNode.Next;
                        if (je.Joint.IslandFlag)
                        {
                            continue;
                        }

                        var other = je.Other;

                        // Don't simulate joints connected to inactive bodies.
                        // 跳过闲置物体
                        if (other.IsActive == false)
                        {
                            continue;
                        }

                        island.Add(je.Joint);
                        je.Joint.IslandFlag = true;

                        if (other.HasFlag(BodyFlags.Island))
                        {
                            continue;
                        }

                        //Debug.Assert(stackCount < stackSize);
                        stack.Push(other);
                        other.SetFlag(BodyFlags.Island);
                    }
                }

                // 岛屿碰撞求解
                island.Solve(out var profile, step, Gravity, AllowSleep);
                Profile.SolveInit += profile.SolveInit;
                Profile.SolveVelocity += profile.SolveVelocity;
                Profile.SolvePosition += profile.SolvePosition;

                // Post solve cleanup.
                for (var i = 0; i < island.BodyCount; ++i)
                {
                    // Allow static bodies to participate in other islands.
                    var b = island.Bodies[i];
                    if (b.BodyType == BodyType.StaticBody)
                    {
                        b.UnsetFlag(BodyFlags.Island);
                    }
                }
            }

            {
                _solveTimer.Restart();

                // Synchronize fixtures, check for out of range bodies.
                bodyNode = BodyList.First;
                while (bodyNode != null)
                {
                    var b = bodyNode.Value;
                    bodyNode = bodyNode.Next;

                    // If a body was not in an island then it did not move.
                    if (!b.HasFlag(BodyFlags.Island))
                    {
                        continue;
                    }

                    if (b.BodyType == BodyType.StaticBody)
                    {
                        continue;
                    }

                    // Update fixtures (for broad-phase).
                    b.SynchronizeFixtures();
                }

                // Look for new contacts.
                ContactManager.FindNewContacts();
                _solveTimer.Stop();
                Profile.Broadphase = _solveTimer.ElapsedMilliseconds;
            }
            island.Reset();
        }

        /// <summary>
        /// Find TOI contacts and solve them.
        /// 求解碰撞时间
        /// </summary>
        /// <param name="step"></param>
        private void SolveTOI(in TimeStep step)
        {
            var island = new Island(
                2 * Settings.MaxToiContacts,
                Settings.MaxToiContacts,
                0,
                ContactManager.ContactListener);

            if (_stepComplete)
            {
                var bodyNode = BodyList.First;
                while (bodyNode != null)
                {
                    var b = bodyNode.Value;
                    bodyNode = bodyNode.Next;
                    b.UnsetFlag(BodyFlags.Island);
                    b.Sweep.Alpha0 = 0.0f;
                }

                var contactNode = ContactManager.ContactList.First;
                while (contactNode != null)
                {
                    var c = contactNode.Value;
                    contactNode = contactNode.Next;

                    // Invalidate TOI
                    c.Flags &= ~(Contact.ContactFlag.ToiFlag | Contact.ContactFlag.IslandFlag);
                    c.ToiCount = 0;
                    c.Toi = 1.0f;
                }
            }

            // Find TOI events and solve them.
            for (;;)
            {
                // Find the first TOI.
                Contact minContact = null;
                var minAlpha = 1.0f;

                var contactNode = ContactManager.ContactList.First;
                while (contactNode != null)
                {
                    var c = contactNode.Value;
                    contactNode = contactNode.Next;

                    // Is this contact disabled?
                    if (c.IsEnabled == false)
                    {
                        continue;
                    }

                    // Prevent excessive sub-stepping.
                    if (c.ToiCount > Settings.MaxSubSteps)
                    {
                        continue;
                    }

                    var alpha = 1.0f;
                    if (c.HasFlag(Contact.ContactFlag.ToiFlag))
                    {
                        // This contact has a valid cached TOI.
                        alpha = c.Toi;
                    }
                    else
                    {
                        var fA = c.FixtureA;
                        var fB = c.FixtureB;

                        // Is there a sensor?
                        // 如果接触点的夹具是传感器,不参与TOI计算,跳过
                        if (fA.IsSensor || fB.IsSensor)
                        {
                            continue;
                        }

                        var bA = fA.Body;
                        var bB = fB.Body;

                        var typeA = bA.BodyType;
                        var typeB = bB.BodyType;
                        Debug.Assert(typeA == BodyType.DynamicBody || typeB == BodyType.DynamicBody);

                        var activeA = bA.IsAwake && typeA != BodyType.StaticBody;
                        var activeB = bB.IsAwake && typeB != BodyType.StaticBody;

                        // Is at least one body active (awake and dynamic or kinematic)?
                        if (activeA == false && activeB == false)
                        {
                            continue;
                        }

                        var collideA = bA.IsBullet || typeA != BodyType.DynamicBody;
                        var collideB = bB.IsBullet || typeB != BodyType.DynamicBody;

                        // Are these two non-bullet dynamic bodies?
                        if (collideA == false && collideB == false)
                        {
                            continue;
                        }

                        // Compute the TOI for this contact.
                        // Put the sweeps onto the same time interval.
                        var alpha0 = bA.Sweep.Alpha0;

                        if (bA.Sweep.Alpha0 < bB.Sweep.Alpha0)
                        {
                            alpha0 = bB.Sweep.Alpha0;
                            bA.Sweep.Advance(alpha0);
                        }
                        else if (bB.Sweep.Alpha0 < bA.Sweep.Alpha0)
                        {
                            alpha0 = bA.Sweep.Alpha0;
                            bB.Sweep.Advance(alpha0);
                        }

                        Debug.Assert(alpha0 < 1.0f);

                        var indexA = c.ChildIndexA;
                        var indexB = c.ChildIndexB;

                        // Compute the time of impact in interval [0, minTOI]
                        var input = new ToiInput();
                        input.ProxyA.Set(fA.Shape, indexA);
                        input.ProxyB.Set(fB.Shape, indexB);
                        input.SweepA = bA.Sweep;
                        input.SweepB = bB.Sweep;
                        input.Tmax = 1.0f;

                        TimeOfImpact.ComputeTimeOfImpact(out var output, input, ToiProfile, GJkProfile);

                        // Beta is the fraction of the remaining portion of the .
                        var beta = output.Time;
                        alpha = output.State == ToiOutput.ToiState.Touching ? Math.Min(alpha0 + (1.0f - alpha0) * beta, 1.0f) : 1.0f;

                        c.Toi = alpha;
                        c.Flags |= Contact.ContactFlag.ToiFlag;
                    }

                    if (alpha < minAlpha)
                    {
                        // This is the minimum TOI found so far.
                        minContact = c;
                        minAlpha = alpha;
                    }
                }

                if (minContact == default || 1.0f - 10.0f * Settings.Epsilon < minAlpha)
                {
                    // No more TOI events. Done!
                    _stepComplete = true;
                    break;
                }

                // Advance the bodies to the TOI.
                var fixtureA = minContact.FixtureA;
                var fixtureB = minContact.FixtureB;
                var bodyA = fixtureA.Body;
                var bodyB = fixtureB.Body;

                var backup1 = bodyA.Sweep;
                var backup2 = bodyB.Sweep;

                bodyA.Advance(minAlpha);
                bodyB.Advance(minAlpha);

                // The TOI contact likely has some new contact points.
                minContact.Update(ContactManager.ContactListener);
                minContact.Flags &= ~Contact.ContactFlag.ToiFlag;
                ++minContact.ToiCount;

                // Is the contact solid?
                if (minContact.IsEnabled == false || minContact.IsTouching == false)
                {
                    // Restore the sweeps.
                    minContact.SetEnabled(false);
                    bodyA.Sweep = backup1;
                    bodyB.Sweep = backup2;
                    bodyA.SynchronizeTransform();
                    bodyB.SynchronizeTransform();
                    continue;
                }

                bodyA.IsAwake = true;
                bodyB.IsAwake = true;

                // Build the island
                island.Clear();
                island.Add(bodyA);
                island.Add(bodyB);
                island.Add(minContact);

                bodyA.SetFlag(BodyFlags.Island);
                bodyB.SetFlag(BodyFlags.Island);
                minContact.Flags |= Contact.ContactFlag.IslandFlag;

                // Get contacts on bodyA and bodyB.
                {
                    var body = bodyA;
                    if (body.BodyType == BodyType.DynamicBody)
                    {
                        var node = body.ContactEdges.First;
                        while (node != null)
                        {
                            var contactEdge = node.Value;
                            node = node.Next;

                            if (island.BodyCount == island.Bodies.Length)
                            {
                                break;
                            }

                            if (island.ContactCount == island.Contacts.Length)
                            {
                                break;
                            }

                            var contact = contactEdge.Contact;

                            // Has this contact already been added to the island?
                            if (contact.HasFlag(Contact.ContactFlag.IslandFlag))
                            {
                                continue;
                            }

                            // Only add static, kinematic, or bullet bodies.
                            var other = contactEdge.Other;
                            if (other.BodyType == BodyType.DynamicBody
                             && body.IsBullet == false
                             && other.IsBullet == false)
                            {
                                continue;
                            }

                            // Skip sensors.
                            var sensorA = contact.FixtureA.IsSensor;
                            var sensorB = contact.FixtureB.IsSensor;
                            if (sensorA || sensorB)
                            {
                                continue;
                            }

                            // Tentatively advance the body to the TOI.
                            var backup = other.Sweep;
                            if (!other.HasFlag(BodyFlags.Island))
                            {
                                other.Advance(minAlpha);
                            }

                            // Update the contact points
                            contact.Update(ContactManager.ContactListener);

                            // Was the contact disabled by the user?
                            if (contact.IsEnabled == false)
                            {
                                other.Sweep = backup;
                                other.SynchronizeTransform();
                                continue;
                            }

                            // Are there contact points?
                            if (contact.IsTouching == false)
                            {
                                other.Sweep = backup;
                                other.SynchronizeTransform();
                                continue;
                            }

                            // Add the contact to the island
                            contact.Flags |= Contact.ContactFlag.IslandFlag;
                            island.Add(contact);

                            // Has the other body already been added to the island?
                            if (other.HasFlag(BodyFlags.Island))
                            {
                                continue;
                            }

                            // Add the other body to the island.
                            other.SetFlag(BodyFlags.Island);

                            if (other.BodyType != BodyType.StaticBody)
                            {
                                other.IsAwake = true;
                            }

                            island.Add(other);
                        }
                    }
                }
                {
                    var body = bodyB;
                    if (body.BodyType == BodyType.DynamicBody)
                    {
                        var node = body.ContactEdges.First;
                        while (node != null)
                        {
                            var contactEdge = node.Value;
                            node = node.Next;

                            if (island.BodyCount == island.Bodies.Length)
                            {
                                break;
                            }

                            if (island.ContactCount == island.Contacts.Length)
                            {
                                break;
                            }

                            var contact = contactEdge.Contact;

                            // Has this contact already been added to the island?
                            if (contact.HasFlag(Contact.ContactFlag.IslandFlag))
                            {
                                continue;
                            }

                            // Only add static, kinematic, or bullet bodies.
                            var other = contactEdge.Other;
                            if (other.BodyType == BodyType.DynamicBody
                             && body.IsBullet == false
                             && other.IsBullet == false)
                            {
                                continue;
                            }

                            // Skip sensors.
                            var sensorA = contact.FixtureA.IsSensor;
                            var sensorB = contact.FixtureB.IsSensor;
                            if (sensorA || sensorB)
                            {
                                continue;
                            }

                            // Tentatively advance the body to the TOI.
                            var backup = other.Sweep;
                            if (!other.HasFlag(BodyFlags.Island))
                            {
                                other.Advance(minAlpha);
                            }

                            // Update the contact points
                            contact.Update(ContactManager.ContactListener);

                            // Was the contact disabled by the user?
                            if (contact.IsEnabled == false)
                            {
                                other.Sweep = backup;
                                other.SynchronizeTransform();
                                continue;
                            }

                            // Are there contact points?
                            if (contact.IsTouching == false)
                            {
                                other.Sweep = backup;
                                other.SynchronizeTransform();
                                continue;
                            }

                            // Add the contact to the island
                            contact.Flags |= Contact.ContactFlag.IslandFlag;
                            island.Add(contact);

                            // Has the other body already been added to the island?
                            if (other.HasFlag(BodyFlags.Island))
                            {
                                continue;
                            }

                            // Add the other body to the island.
                            other.SetFlag(BodyFlags.Island);

                            if (other.BodyType != BodyType.StaticBody)
                            {
                                other.IsAwake = true;
                            }

                            island.Add(other);
                        }
                    }
                }

                var dt = (1.0f - minAlpha) * step.Dt;
                var subStep = new TimeStep
                {
                    Dt = dt, InvDt = 1.0f / dt,
                    DtRatio = 1.0f, PositionIterations = 20,
                    VelocityIterations = step.VelocityIterations, WarmStarting = false
                };

                island.SolveTOI(subStep, bodyA.IslandIndex, bodyB.IslandIndex);

                // Reset island flags and synchronize broad-phase proxies.
                for (var i = 0; i < island.BodyCount; ++i)
                {
                    var body = island.Bodies[i];
                    body.UnsetFlag(BodyFlags.Island);

                    if (body.BodyType != BodyType.DynamicBody)
                    {
                        continue;
                    }

                    body.SynchronizeFixtures();

                    // Invalidate all contact TOIs on this displaced body.
                    var node = bodyB.ContactEdges.First;
                    while (node != null)
                    {
                        node.Value.Contact.Flags &= ~(Contact.ContactFlag.ToiFlag | Contact.ContactFlag.IslandFlag);
                        node = node.Next;
                    }
                }

                // Commit fixture proxy movements to the broad-phase so that new contacts are created.
                // Also, some contacts can be destroyed.
                ContactManager.FindNewContacts();

                if (SubStepping)
                {
                    _stepComplete = false;
                    break;
                }
            }

            island.Reset();
        }

        /// Dump the world into the log file.
        /// @warning this should be called outside of a time step.
        public void Dump()
        {
            if (IsLocked)
            {
                return;
            }

            DumpLogger.Log($"gravity = ({Gravity.X}, {Gravity.Y});");
            DumpLogger.Log($"bodies  = {BodyList.Count};");
            DumpLogger.Log($"joints  = {JointList.Count};");
            var i = 0;
            foreach (var b in BodyList)
            {
                b.IslandIndex = i;
                b.Dump();
                ++i;
            }

            i = 0;
            foreach (var j in JointList)
            {
                j.Index = i;
                ++i;
            }

            // First pass on joints, skip gear joints.
            foreach (var j in JointList)
            {
                if (j.JointType == JointType.GearJoint)
                {
                    continue;
                }

                DumpLogger.Log("{");
                j.Dump();
                DumpLogger.Log("}");
            }

            // Second pass on joints, only gear joints.
            foreach (var j in JointList)
            {
                if (j.JointType != JointType.GearJoint)
                {
                    continue;
                }

                DumpLogger.Log("{");
                j.Dump();
                DumpLogger.Log("}");
            }
        }

      
    }
}