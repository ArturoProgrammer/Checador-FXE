using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checador_FXE
{
    public partial class popUpComboBoxEntry : Form
    {
        public string Response { get; protected private set; }

        public popUpComboBoxEntry(DateOnly _fecha, string[] _options, string? _actualSelection = "-1")
        {
            InitializeComponent();
            this.Text = this.Text.Replace("%FECHA%", $"{_fecha:dd/MM/yyyy}");
            foreach (string option in _options)
            {
                this.cboxEntry.Items.Add(option);
            }

            if (_actualSelection != null && _actualSelection != "-1")
            {
                this.cboxEntry.Value = _actualSelection;
            }
        }

        private void popUpComboBoxEntry_Load(object sender, EventArgs e)
        {
            
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.cboxEntry.IsNonSelectedTextSelected)
                return;

            Response = this.cboxEntry.Value;
            this.DialogResult = DialogResult.OK;
        }

        private void cboxEntry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboxEntry.IsNonSelectedTextSelected)
                this.btnAceptar.Enabled = false;

            this.btnAceptar.Enabled = true;
        }
    }
}
