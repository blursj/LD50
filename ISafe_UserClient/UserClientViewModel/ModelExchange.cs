using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISafe_Model;

namespace UserClientViewModel
{
    public class ModelExchange
    {
        public static PipeSiteModel ExchangeToWPF(ServiceReference1.PipeSite wcfmodel)
        {
            if (wcfmodel == null)
            {
                return null;
            }
            PipeSiteModel PipeSite = new PipeSiteModel();

            PipeSite.SiteIndex = wcfmodel.SiteIndex;
            PipeSite.Name = wcfmodel.SiteName;
            PipeSite.PressureReq = wcfmodel.PressureReq;
            PipeSite.IsLink = true;
            PipeSite.NodeKey = wcfmodel.SiteIndex;
            PipeSite.SamplePerBlock = wcfmodel.SamplePerBlock;
            PipeSite.DataBufferSize = wcfmodel.DataBufferSize;

            foreach (var sensor in wcfmodel.PreSensors)
            {
                PreSensorModel PressureSensor = ExchangeToWPF(sensor);
                PressureSensor.Parent = PipeSite;
                PipeSite.Children_next.Add(PressureSensor);
            }

            return PipeSite;
        }

        public static ServiceReference1.PipeSite ExchangeToWCF(PipeSiteModel wpfmodel)
        {
            if (wpfmodel == null)
            {
                return null;
            }

            ServiceReference1.PipeSite Wcf_pipesite = new ServiceReference1.PipeSite();
            Wcf_pipesite.SiteIndex = wpfmodel.SiteIndex;
            Wcf_pipesite.SiteName = wpfmodel.Name;
            Wcf_pipesite.PressureReq = wpfmodel.PressureReq;
            Wcf_pipesite.DataBufferSize = wpfmodel.DataBufferSize;
            Wcf_pipesite.SamplePerBlock = wpfmodel.SamplePerBlock;

            Wcf_pipesite.PreSensors = new ServiceReference1.PressureSensor[wpfmodel.Children_next.Count];
            for (int sor_sign = 0; sor_sign < wpfmodel.Children_next.Count; sor_sign++)
            {
                Wcf_pipesite.PreSensors[sor_sign] = ExchangeToWCF(wpfmodel.Children_next[sor_sign] as PreSensorModel);
            }

            return Wcf_pipesite;
        }

        /// <summary>
        /// 转换为WCF模型
        /// </summary>
        /// <param name="pipedata"></param>
        /// <returns></returns>
        public static ServiceReference1.Pipe ExchangedToWCF(PipeModel pipedata)
        {
            ServiceReference1.Pipe wcf_pipe = new ServiceReference1.Pipe();

            if (pipedata == null)
            {
                return wcf_pipe;
            }
            wcf_pipe.PipeIndex = pipedata.PipeIndex;
            wcf_pipe.PipeLength = pipedata.PipeLength;
            wcf_pipe.PipeName = pipedata.PipeName;

            wcf_pipe.Speed0 = pipedata.Speed0;
            wcf_pipe.Speed1 = pipedata.Speed1;
            wcf_pipe.TimeThreshold = pipedata.TimeThreshold;

            wcf_pipe.Segments = new ServiceReference1.Segment[pipedata.Segments.Count];
            for (int i = 0; i < pipedata.Segments.Count; i++)
            {
                wcf_pipe.Segments[i] = ExchangeToWCF(pipedata.Segments[i]);
            }

            return wcf_pipe;
        }


        public static PipeModel ExchangedToWPF(ServiceReference1.Pipe wcf_pipe)
        {
            PipeModel wpfpipe = new PipeModel();
            if (wcf_pipe == null)
            {
                return wpfpipe;
            }

            wpfpipe.PipeIndex = wcf_pipe.PipeIndex;
            wpfpipe.PipeLength = wcf_pipe.PipeLength;
            wpfpipe.PipeName = wcf_pipe.PipeName;

            wpfpipe.Speed0 = wcf_pipe.Speed0;
            wpfpipe.Speed1 = wcf_pipe.Speed1;
            wpfpipe.TimeThreshold = wcf_pipe.TimeThreshold;

            for (int i = 0; i < wcf_pipe.Segments.Length; i++)
            {
                wpfpipe.Segments.Add(ExchangeToWPF(wcf_pipe.Segments[i]));
            }
            return wpfpipe;
        }

        public static ServiceReference1.Segment ExchangeToWCF(Segment segment)
        {
            ServiceReference1.Segment wcf_segment = new ServiceReference1.Segment();
            if (segment == null)
            {
                return wcf_segment;
            }

            wcf_segment.Name = segment.Name;
            wcf_segment.Stroke = segment.Stroke;
            wcf_segment.X1 = segment.X1;
            wcf_segment.X2 = segment.X2;
            wcf_segment.Y1 = segment.Y1;
            wcf_segment.Y2 = segment.Y2;

            return wcf_segment;
        }

        public static Segment ExchangeToWPF(ServiceReference1.Segment wcfmodel)
        {
            Segment segment = new Segment();

            if (wcfmodel == null)
            {
                return segment;
            }

            segment.Name = wcfmodel.Name;
            segment.Stroke = wcfmodel.Stroke;
            segment.X1 = wcfmodel.X1;
            segment.X2 = wcfmodel.X2;
            segment.Y1 = wcfmodel.Y1;
            segment.Y2 = wcfmodel.Y2;

            return segment;
        }

        public static PreSensorModel ExchangeToWPF(ServiceReference1.PressureSensor wcfmolde)
        {

            PreSensorModel wpfmodel = new PreSensorModel();

            if (wcfmolde != null)
            {
                wpfmodel.PreSensorID = wcfmolde.PreSensorID;
                wpfmodel.OPCPointID = wcfmolde.OPCPointID;
                wpfmodel.ThreshMax = wcfmolde.ThreshMax;
                wpfmodel.ThreshMin = wcfmolde.ThreshMin;
                wpfmodel.IsLink = wcfmolde.IsLinked;
                wpfmodel.IsSaveData = wcfmolde.IsSaveData;
                wpfmodel.AllSendToClient = wcfmolde.AllSendToClient;
                wpfmodel.DataType = NodeType.PreSensor;
                wpfmodel.NodeKey = wcfmolde.PreSensorID;
                wpfmodel.Name = "传感器" + wcfmolde.PreSensorID;

                if (wcfmolde.MonitorPipeSegments != null)
                {
                    foreach (var item in wcfmolde.MonitorPipeSegments)
                    {
                        SegmentPipe segmentpipe = new SegmentPipe();
                        segmentpipe.SegmentLocation = item.SegmentLocation;
                        var find_pipe = MainWindowViewModel.Instance.PipesManager.TotalPipes.FirstOrDefault(para => para.PipeIndex == Convert.ToInt32(item.SegmentIndex));
                        if (find_pipe != null)
                        {
                            segmentpipe.MoniterPipe = find_pipe;
                        }
                        segmentpipe.BelongPreSensor = wpfmodel;
                        wpfmodel.SegmentPipes.Add(segmentpipe);
                    }
                }
            }

            return wpfmodel;
        }

        public static ServiceReference1.PressureSensor ExchangeToWCF(PreSensorModel wpfmodel)
        {
            ServiceReference1.PressureSensor wcfmodel = new ServiceReference1.PressureSensor();

            if (wpfmodel != null)
            {
                wcfmodel.PreSensorID = wpfmodel.PreSensorID;
                wcfmodel.OPCPointID = wpfmodel.OPCPointID;
                wcfmodel.IsLinked = wpfmodel.IsLink;
                wcfmodel.ThreshMax = wpfmodel.ThreshMax;
                wcfmodel.ThreshMin = wpfmodel.ThreshMin;
                wcfmodel.AllSendToClient = wpfmodel.AllSendToClient;
                wcfmodel.IsSaveData = wpfmodel.IsSaveData;

                if (wpfmodel.Parent != null)
                {
                    wcfmodel.BelongSiteID = (wpfmodel.Parent as PipeSiteModel).SiteIndex;
                }

                if (wpfmodel.SegmentPipes != null)
                {
                    wcfmodel.MonitorPipeSegments = new ServiceReference1.pipesegment[wpfmodel.SegmentPipes.Count];
                    for (int i = 0; i < wpfmodel.SegmentPipes.Count; i++)
                    {
                        ServiceReference1.pipesegment segment = new ServiceReference1.pipesegment();

                        if (wpfmodel.SegmentPipes[i].MoniterPipe != null)
                        {
                            segment.SegmentIndex = wpfmodel.SegmentPipes[i].MoniterPipe.PipeIndex.ToString();
                        }

                        segment.SegmentLocation = wpfmodel.SegmentPipes[i].SegmentLocation;

                        wcfmodel.MonitorPipeSegments[i] = segment;
                    }
                }
            }
            return wcfmodel;
        }

        public static OPCProxyModel ExchangeToWPF(ServiceReference1.OPCPxoryModel wcfmodel)
        {
            OPCProxyModel wpfmodel = new OPCProxyModel();
            wpfmodel.GUID = wcfmodel.GUID;
            wpfmodel.Sign = wcfmodel.Sign;
            wpfmodel.Description = wcfmodel.Description;
            wpfmodel.IsLinked = wcfmodel.IsLinked;
            return wpfmodel;
        }

        public static ServiceReference1.OPCPxoryModel ExchangeToWCF(OPCProxyModel wpfmodel)
        {
            ServiceReference1.OPCPxoryModel wcfmodel = new ServiceReference1.OPCPxoryModel();
            wcfmodel.GUID = wpfmodel.GUID;
            wcfmodel.Sign = wpfmodel.Sign;
            wcfmodel.Description = wpfmodel.Description;
            wcfmodel.IsLinked = wpfmodel.IsLinked;
            return wcfmodel;
        }

    }
}
