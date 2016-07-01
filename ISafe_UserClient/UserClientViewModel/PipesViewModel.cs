using ISafe_Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserClientViewModel
{
    public class PipesViewModel : PropertyCallBack
    {
        public PipesViewModel()
        {
            _Pipes = new ObservableCollection<PipeModel>();
            _CombinePipes = new ObservableCollection<PipeModel>();
            _TotalPipes = new ObservableCollection<PipeModel>();
        }

        private ObservableCollection<PipeModel> _Pipes;
        /// <summary>
        /// 原始管段集合
        /// </summary>
        public ObservableCollection<PipeModel> Pipes
        {
            get
            {
                return _Pipes;
            }
            set
            {
                _Pipes = value;
                OnPropertyChanged("Pipes");
            }
        }

        private ObservableCollection<PipeModel> _CombinePipes;
        /// <summary>
        /// 组合管段集合
        /// </summary>
        public ObservableCollection<PipeModel> CombinePipes
        {
            get
            {
                return _CombinePipes;
            }
            set
            {
                _CombinePipes = value;
                OnPropertyChanged("CombinePipes");
            }
        }

        private ObservableCollection<PipeModel> _TotalPipes;
        public ObservableCollection<PipeModel> TotalPipes
        {
            get
            {
                return _TotalPipes;
            }
            set
            {
                _TotalPipes = value;
                OnPropertyChanged("TotalPipes");
            }
        }

        private DelegateCommand _AddCombinePipe;
        /// <summary>
        /// 增加组合管段
        /// </summary>
        public DelegateCommand AddCombinePipe
        {
            get
            {
                if (_AddCombinePipe == null)
                {
                    _AddCombinePipe = new DelegateCommand((obj) =>
                    {
                        PipeModel newcombinepipe = new PipeModel();
                        int pipeindex = Pipes.Count + CombinePipes.Count;
                        var findsaveindex = CombinePipes.FirstOrDefault(para => para.PipeIndex == pipeindex);
                        while (findsaveindex != null)
                        {
                            pipeindex += 1;
                            findsaveindex = CombinePipes.FirstOrDefault(para => para.PipeIndex == pipeindex);
                        }

                        newcombinepipe.PipeIndex = pipeindex;
                        CombinePipes.Add(newcombinepipe);
                        TotalPipes.Add(newcombinepipe);

                        MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:增加组合管段成功！", DateTime.Now.ToString()));
                    });
                }
                return _AddCombinePipe;
            }
        }

        private DelegateCommand _RemoveCombinePipe;
        /// <summary>
        /// 删除一个组合管段
        /// </summary>
        public DelegateCommand RemoveCombimePipe
        {
            get
            {
                if (_RemoveCombinePipe == null)
                {
                    _RemoveCombinePipe = new DelegateCommand((obj) =>
                    {
                        PipeModel selectedPipe = obj as PipeModel;
                        if (selectedPipe != null)
                        {
                            try
                            {
                                CombinePipes.Remove(selectedPipe);
                                TotalPipes.Remove(selectedPipe);
                                MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:删除ID号为{1}的管段成功！", DateTime.Now.ToString(), selectedPipe.PipeIndex));
                            }
                            catch
                            {
                                MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:删除ID号为{1}的管段失败！", DateTime.Now.ToString(), selectedPipe.PipeIndex));
                                //填写日志信息
                            }

                        }
                    });
                }
                return _RemoveCombinePipe;
            }
        }

        private DelegateCommand _SavePipeSet;
        /// <summary>
        /// 保存管段设置
        /// </summary>
        public DelegateCommand SavePipeSet
        {
            get
            {
                if (_SavePipeSet == null)
                {
                    _SavePipeSet = new DelegateCommand((obj) =>
                    {
                        List<ServiceReference1.Pipe> WCFCombinePipes = new List<ServiceReference1.Pipe>();
                        for (int i = 0; i < CombinePipes.Count; i++)
                        {
                            WCFCombinePipes.Add(ModelExchange.ExchangedToWCF(CombinePipes[i]));
                        }
                        if (MainWindowViewModel.Instance.WCFManager.ServerGrapic != null)
                        {
                            MainWindowViewModel.Instance.WCFManager.ServerGrapic.CombinPipes = WCFCombinePipes.ToArray<ServiceReference1.Pipe>();
                        }
                        lock (MainWindowViewModel.Instance.WCFManager._ClientLock)
                        {
                            try
                            {
                                MainWindowViewModel.Instance.WCFManager.Client.SetGraphic(MainWindowViewModel.Instance.WCFManager.ServerGrapic);
                                MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:保存管段设置成功！", DateTime.Now.ToString()));
                            }
                            catch
                            {
                                MainWindowViewModel.Instance.RuningMSG.Add(string.Format("{0}:保存管段设置失败！", DateTime.Now.ToString()));
                            }

                        }
                    });
                }
                return _SavePipeSet;
            }
        }
    }
}
