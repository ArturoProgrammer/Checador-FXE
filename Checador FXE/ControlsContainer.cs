using FlowControls;

namespace Checador_FXE
{
    public class ControlsContainer : FlowLayoutPanel
    {
        private flLabelHeader lblHeader;


        public ControlsContainer()
        {
            lblHeader = new flLabelHeader()
            {
                Dock = DockStyle.Top,
                HeaderText = "Controles",
                Margin = new Padding(0, 0, 0, 5),
            };
            this.Controls.Add(lblHeader);

            this.Dock = DockStyle.Fill;
            this.FlowDirection = FlowDirection.TopDown;
            this.WrapContents = false;
            this.AutoScroll = true;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            // El header se debe de ajustar al ancho del contenedor
            lblHeader.Width = this.ClientSize.Width - this.Padding.Left - this.Padding.Right;
        }
    }
}
