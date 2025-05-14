namespace AplicacionTFG.Presentation.TabBar
{
	public sealed partial class TabBarPequeño : UserControl
	{
        public TabBarPequeño()
        {
            this.InitializeComponent();        
        }
            
      
        private bool isExpanded = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isExpanded)
                CollapseTabBar();
            else
                ExpandTabBar();
            isExpanded = !isExpanded;
            
        }

        private void ExpandTabBar()
        {
            AbrirTabBar.Begin();
        }

        private void CollapseTabBar()
        {
            CerrarTabBar.Begin();
        }
    }
}
