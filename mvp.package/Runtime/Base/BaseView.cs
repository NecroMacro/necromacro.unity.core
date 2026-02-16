using UnityEngine;

namespace NecroMacro.MVP
{
	public interface IBaseView
	{
		public void Show();
		public void Close();
	}
	
	public abstract class BaseView : MonoBehaviour, IBaseView
	{
		public virtual void Show()
		{
			gameObject.SetActive(true);
		}

		public virtual void Close()
		{
			gameObject.SetActive(false);
		}
	}
}