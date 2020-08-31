using System;
using System.Windows.Controls;
using System.Diagnostics;

namespace GameWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow
    {
        readonly System.Windows.Threading.DispatcherTimer m_renderTimer; // Updates the main window at 60fps.
        readonly Stopwatch m_stopwatch; // Monitor elapsed time between frames.
        readonly Canvas m_canvas; // Stores the polygons ready for rendering to the main window.
        readonly World m_world; // The game world.
        
        /// <summary>
        /// Constructor for MainWindow. Creates the main game world and updates it at a regular interval.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            m_canvas = new Canvas();
            Content = m_canvas;

            m_world = new World();
            m_stopwatch = Stopwatch.StartNew();
            
            m_renderTimer = new System.Windows.Threading.DispatcherTimer();
            m_renderTimer.Interval = TimeSpan.FromSeconds(1.0f / 30.0f);
            m_renderTimer.Tick += Update;
            m_renderTimer.Start();
        }

        /// <summary>
        /// Updates the game world and re-renders it. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update(object sender, EventArgs e)
        {
            m_canvas.Children.RemoveRange(0, 100);
            m_world.Update(m_stopwatch.ElapsedMilliseconds / 1000.0f);
            m_world.Draw(m_canvas.Children);
            m_stopwatch.Restart();
        }
    }
}