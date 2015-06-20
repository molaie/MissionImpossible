using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace MI {
	public static class Utilities {
		public static void InvokeIfRequired(this DispatcherObject control, Action<DispatcherObject> action, object[] args) {
			if (control.CheckAccess()) {
				control.Dispatcher.Invoke(new Action(() => action(control)));
			} else {
				action(control);
			}
		}
	}
}
