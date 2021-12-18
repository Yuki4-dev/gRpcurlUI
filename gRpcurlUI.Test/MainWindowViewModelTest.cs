using gRpcurlUI.Model;
using gRpcurlUI.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.Test
{
    [TestClass]
    public class MainWindowViewModelTest
    {

        [TestMethod]
        public void SelectCommand_1()
        {
            var mock = new Mock<IExecutePageViewModel<IProject>>();
            IExecutePageViewModel<IProject> executePageViewModel = mock.Object;

            var target = new TabContentPageViewModel(null, executePageViewModel);
            var project = new Mock<IProject>().Object;

            target.SelectedCommand.Execute(project);

            mock.VerifySet(m => m.SelectedProject = project, Times.Once);
        }

        [TestMethod]
        public void AddCommand_1()
        {
            var mock = new Mock<IExecutePageViewModel<IProject>>();
            IExecutePageViewModel<IProject> executePageViewModel = mock.Object;

            var target = new TabContentPageViewModel(null, executePageViewModel);
            target.AddCommand.Execute(null);

            mock.Verify(m => m.Add(It.IsAny<IProject>()), Times.Once);
        }

        [TestMethod]
        public void RemoveCommand_1()
        {
            var mock = new Mock<IExecutePageViewModel<IProject>>();
            IExecutePageViewModel<IProject> executePageViewModel = mock.Object;

            var target = new TabContentPageViewModel(null, executePageViewModel);
            target.ShowMessageDialog += (m, b) =>
            {
                return Task.FromResult(MessageBoxResult.Yes);
            };


            target.RemoveCommand.Execute(null);
            Assert.AreEqual(target.IsRemoveMode, true);

            var p1 = new Mock<IProject>().Object;
            var p2 = new Mock<IProject>().Object;
            target.SelectedCommand.Execute(p1);
            target.SelectedCommand.Execute(p2);

            target.RemoveCommand.Execute(null);
            Assert.AreEqual(target.IsRemoveMode, false);

            mock.Verify(m => m.Remove(p1), Times.Once);
            mock.Verify(m => m.Remove(p2), Times.Once);
        }

        [TestMethod]
        public void RemoveCommand_2()
        {
            var mock = new Mock<IExecutePageViewModel<IProject>>();
            IExecutePageViewModel<IProject> executePageViewModel = mock.Object;

            var target = new TabContentPageViewModel(null, executePageViewModel);
            target.ShowMessageDialog += (m, b) =>
            {
                return Task.FromResult(MessageBoxResult.No);
            };


            target.RemoveCommand.Execute(null);
            Assert.AreEqual(target.IsRemoveMode, true);

            var p1 = new Mock<IProject>().Object;
            var p2 = new Mock<IProject>().Object;
            target.SelectedCommand.Execute(p1);
            target.SelectedCommand.Execute(p2);

            target.RemoveCommand.Execute(null);
            Assert.AreEqual(target.IsRemoveMode, false);

            mock.Verify(m => m.Remove(p1), Times.Never);
            mock.Verify(m => m.Remove(p2), Times.Never);
        }

        [TestMethod]
        public void ImportCommand_1()
        {
            var mockExecutePageVm = new Mock<IExecutePageViewModel<IProject>>();
            IExecutePageViewModel<IProject> executePageViewModel = mockExecutePageVm.Object;

            var mockLoadModel = new Mock<ILoadModel>();
            ILoadModel loadModel = mockLoadModel.Object;

            var mockContext = new Mock<IProjectContext<IProject>>();
            IProjectContext<IProject> projectContext = mockContext.Object;

            var p1 = new Mock<IProject>().Object;
            var p2 = new Mock<IProject>().Object;

            var target = new TabContentPageViewModel(loadModel, executePageViewModel);
            target.ShowCommonDialog += (t, act1, act2) =>
            {
                Assert.AreEqual(t, typeof(OpenFileDialog));
                var ofd = (OpenFileDialog)Activator.CreateInstance(t);
                ofd.FileName = "Sample.json";
                act2.Invoke(ofd);
                return Task.FromResult(true);
            };

            mockContext.Setup(m => m.Verion).Returns("1");
            mockExecutePageVm.Setup(m => m.Context.Verion).Returns("1");
            mockLoadModel.Setup(m => m.Load("Sample.json", It.IsAny<Type>())).Returns(projectContext);
            mockContext.Setup(m => m.Projects).Returns(new[] { p1, p2 });

            target.ImportCommand.Execute(null);

            mockExecutePageVm.Verify(m => m.Add(p1), Times.Once);
            mockExecutePageVm.Verify(m => m.Add(p2), Times.Once);
        }

        [TestMethod]
        public void ImportCommand_2()
        {
            var mockExecutePageVm = new Mock<IExecutePageViewModel<IProject>>();
            IExecutePageViewModel<IProject> executePageViewModel = mockExecutePageVm.Object;

            var mockLoadModel = new Mock<ILoadModel>();
            ILoadModel loadModel = mockLoadModel.Object;

            var mockContext = new Mock<IProjectContext<IProject>>();
            IProjectContext<IProject> projectContext = mockContext.Object;

            var p1 = new Mock<IProject>().Object;
            var p2 = new Mock<IProject>().Object;

            var target = new TabContentPageViewModel(loadModel, executePageViewModel);
            target.ShowCommonDialog += (t, act1, act2) =>
            {
                Assert.AreEqual(t, typeof(OpenFileDialog));
                var ofd = (OpenFileDialog)Activator.CreateInstance(t);
                ofd.FileName = "Sample.json";
                act2.Invoke(ofd);
                return Task.FromResult(true);
            };

            var errorCalled = false;
            target.ShowMessageDialog += (msg, btn) =>
            {
                errorCalled = true;
                return Task.FromResult(MessageBoxResult.None);
            };

            mockContext.Setup(m => m.Verion).Returns("1");
            mockExecutePageVm.Setup(m => m.Context.Verion).Returns("2");
            mockLoadModel.Setup(m => m.Load("Sample.json", It.IsAny<Type>())).Returns(projectContext);

            target.ImportCommand.Execute(null);

            Assert.AreEqual(errorCalled, true);
        }
    }
}