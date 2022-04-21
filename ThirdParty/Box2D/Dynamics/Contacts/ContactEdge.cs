using System.Collections.Generic;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts
{
    /// A contact edge is used to connect bodies and contacts together
    /// in a contact graph where each body is a node and each contact
    /// is an edge. A contact edge belongs to a doubly linked list
    /// maintained in each attached body. Each contact has two contact
    /// nodes, one for each attached body.
    public class ContactEdge
    {
        /// provides quick access to the other body attached.
        public Body Other;

        /// the contact        
        public Contact Contact;

        public readonly LinkedListNode<ContactEdge> Node = new LinkedListNode<ContactEdge>(default);
    }
}