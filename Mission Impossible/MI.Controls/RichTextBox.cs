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



		public void Select(int startPos, int EndPos) {
			TextRange temp = new TextRange(this.Document.ContentStart, this.Document.ContentEnd);
			TextPointer start = temp.Start.GetPositionAtOffset(startPos);
			TextPointer end = start.GetPositionAtOffset(EndPos - startPos);
			this.Selection.Select(start, end);
		}


		public string GetText(int startPos, int EndPos) {
			TextRange temp = new TextRange(this.Document.ContentStart, this.Document.ContentEnd);
			TextPointer start = temp.Start.GetPositionAtOffset(startPos);
			TextPointer end = temp.Start.GetPositionAtOffset(EndPos);
			return new TextRange(start, end).Text;
		}




	}
}
