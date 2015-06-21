using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MI.Controls {
	public class RichTextBox : System.Windows.Controls.RichTextBox {

		public RichTextBox()
			: base() {
		}

		public RichTextBox(FlowDocument document)
			: base(document) {
		}

		public string Text {

			get {
				return new TextRange(this.Document.ContentStart, this.Document.ContentEnd).Text;
			}

		}


	}
}
