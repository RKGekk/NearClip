using BasicRender.Engine;
using MathematicalEntities;
using ObjReader.Data.DataStore;
using ObjReader.Data.Elements;
using ObjReader.Loaders;
using ObjReader.TypeParsers;
using PhysicsEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasicRender {

    public partial class MainWindow : Window {

        private GameTimer _timer = new GameTimer();

        private WriteableBitmap _wbStat;
        private Int32Rect _rectStat;
        private Renderer _rendererStat;

        private WriteableBitmap _wb;
        private Int32Rect _rect;
        private Renderer _renderer;

        private float _lastX;
        private float _lastY;
        private bool _lockMouse;

        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e) {

            _timer.reset();
            _timer.start();

            int pixelWidth = (int)img.Width;
            int pixelHeight = (int)img.Height;

            _lastX = ((float)pixelWidth) / 2.0f;
            _lastY = ((float)pixelHeight) / 2.0f;
            _lockMouse = false;

            _wb = new WriteableBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Bgra32, null);
            _rect = new Int32Rect(0, 0, pixelWidth, pixelHeight);

            byte[] pixels = new byte[pixelWidth * pixelHeight * _wb.Format.BitsPerPixel / 8];
            float[] zbuf = new float[pixelWidth * pixelHeight];
            _renderer = new Renderer(_timer, pixels, zbuf, pixelWidth);
            _renderer.fillScreen(BasicColors.Grey3f);
            _renderer.fillZBuff(1.0f);
            _renderer.setObject("simpleBox.obj");
            //_renderer.setObject("ground1.obj");

            _wb.WritePixels(_rect, pixels, _renderer.stride, 0);

            img.Source = _wb;

            InitializeStats();

            CompositionTarget.Rendering += UpdateChildren;

            MouseDown += MainWindow_MouseDown;
            MouseUp += MainWindow_MouseUp;
            MouseMove += MainWindow_MouseMove;
            LostMouseCapture += MainWindow_LostMouseCapture;
        }

        private void MainWindow_LostMouseCapture(object sender, MouseEventArgs e) {
            
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e) {

            float x = (float)Mouse.GetPosition(this).X;
            float y = (float)Mouse.GetPosition(this).Y;

            if (_lockMouse) {

                float xoffset = x - _lastX;
                float yoffset = y - _lastY;

                _renderer.camera.ProcessMouseMovement(xoffset, yoffset);
            }

            _lastX = x;
            _lastY = y;
        }

        private void MainWindow_MouseUp(object sender, MouseButtonEventArgs e) {
            _lockMouse = false;
            ReleaseMouseCapture();
            Mouse.OverrideCursor = null;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e) {
            _lockMouse = true;
            CaptureMouse();
            Mouse.OverrideCursor = Cursors.None;
        }

        private void InitializeStats() {

            int pixelWidthStat = (int)statImg.Width;
            int pixelHeightStat = (int)statImg.Height;

            _wbStat = new WriteableBitmap(pixelWidthStat, pixelHeightStat, 96, 96, PixelFormats.Bgra32, null);
            _rectStat = new Int32Rect(0, 0, pixelWidthStat, pixelHeightStat);

            byte[] pixelsStat = new byte[pixelWidthStat * pixelHeightStat * _wbStat.Format.BitsPerPixel / 8];

            _rendererStat = new Renderer(_timer, pixelsStat, null, pixelWidthStat);
            _rendererStat.fillScreen(BasicColors.DarkGrey3f);

            _wbStat.WritePixels(_rectStat, pixelsStat, _rendererStat.stride, 0);

            statImg.Source = _wbStat;
        }

        private float _angle = 0.0f;

        protected void UpdateChildren(object sender, EventArgs e) {

            RenderingEventArgs renderingArgs = e as RenderingEventArgs;
            _timer.tick();

            _renderer.fillScreen(BasicColors.Grey3f);
            _renderer.fillZBuff(1.0f);

            float duration = _timer.deltaTime();

            _renderer.renderPolySolidColorZ(false);

            _angle += (float)(Math.PI / 128.0f);

            _wb.WritePixels(_rect, _renderer.buf, _renderer.stride, 0);

            updateStats();
        }

        private void updateStats() {

            float duration = _timer.deltaTime();
            float totalTime = _timer.gameTime();
            int iduration = (int)(duration * 1000.0f);

            statsText.Text = $"RenderDuration: {duration * 1000.0f:F2}ms; FPS: {1.0f / duration:F0}; TotalTime: {totalTime:F3}sec";

            _rendererStat.lmoveScreen(new Vec3f(0.125f, 0.125f, 0.125f), 1);
            
            if (iduration < 32)
                _rendererStat.printPixel(319, iduration, new Vec3f(0.0f, 0.5f, 0.0f));
            _wbStat.WritePixels(_rectStat, _rendererStat.buf, _rendererStat.stride, 0);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {

            if (e.Key == Key.Escape) {
                this.Close();
            }

            if (e.Key == Key.W) {
                this._renderer.camera.ProcessKeyboard(Camera.Camera_Movement.FORWARD, _timer.deltaTime());
            }

            if (e.Key == Key.S) {
                this._renderer.camera.ProcessKeyboard(Camera.Camera_Movement.BACKWARD, _timer.deltaTime());
            }

            if (e.Key == Key.A) {
                this._renderer.camera.ProcessKeyboard(Camera.Camera_Movement.LEFT, _timer.deltaTime());
            }

            if (e.Key == Key.D) {
                this._renderer.camera.ProcessKeyboard(Camera.Camera_Movement.RIGHT, _timer.deltaTime());
            }
        }


    }
}
