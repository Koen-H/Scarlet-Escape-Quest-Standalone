using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// Socket interactor that only selects and hovers interactables with a keychain component containing specific keys.
    /// </summary>
    public class XRLockSocketInteractor : XRSocketInteractor
    {
        [Space]
        [SerializeField]
        [Tooltip("The required keys to interact with this socket.")]
        Lock m_Lock;

        /// <summary>
        /// The required keys to interact with this socket.
        /// </summary>
        public Lock keychainLock
        {
            get => m_Lock;
            set => m_Lock = value;
        }

        /// <inheritdoc />
        public override bool CanHover(IXRHoverInteractable interactable)
        {
            if (!base.CanHover(interactable))
                return false;
            Debug.Log("HOVER TEST" + m_Lock.CanUnlock(interactable.transform.gameObject));
            return m_Lock.CanUnlock(interactable.transform.gameObject);
        }

        /// <inheritdoc />
        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            if (!base.CanSelect(interactable))
                return false;
            //Debug.Log("SELECT TEST" + m_Lock.CanUnlock(interactable.transform.gameObject));
            return m_Lock.CanUnlock(interactable.transform.gameObject);
        }
    }
}
