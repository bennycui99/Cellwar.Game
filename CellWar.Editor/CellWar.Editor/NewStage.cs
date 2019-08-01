using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellWar.Editor {
    public partial class NewStage : Form {
        public NewStage() {
            InitializeComponent();
        }

        private void NewStage_Load( Object sender, EventArgs e ) {
        }

        private void cancel_Click( Object sender, EventArgs e ) {
            this.Close();
        }
    }
}
