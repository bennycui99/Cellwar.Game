using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CellWar.Editor.SixSidesMap;

namespace CellWar.Editor {
    public partial class MainWindow : Form {
        public MainWindow() {
            InitializeComponent();
        }

        private void newStageToolStripMenuItem_Click( Object sender, EventArgs e ) {
            NewStage newStage = new NewStage();
            newStage.ShowDialog();
        }

        private void MainWindow_Load( Object sender, EventArgs e ) {
            SixSidesControl six = new SixSidesControl();
            six.Parent = this;
            six.Width = this.Width;
            six.Height = this.Height;
            six.PointToScreen( new Point(100, 100) );
        }
    }
}
