﻿using System;
using ReaderB;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace ritchell.library.infrastructure.Hardware
{
    public class LongRangeRFID : RFIDReaderBase, IRFIDReader
    {
        private bool ContinueReading;
        private int _Port;
        private byte _ComAddr;
        private byte _Baud;
        private int _PortHandle;

        public LongRangeRFID()
        {
            _ComAddr = Convert.ToByte("FF", 16);
            _Baud = Convert.ToByte("5", 10);
            var ok = StaticClassReaderB.AutoOpenComPort(ref _Port, ref _ComAddr, _Baud, ref _PortHandle);

            if (ok != 0) // zero is sucess
            {
                throw new InvalidOperationException("Port cannot be opened. Long RFID");
            }

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.RunWorkerAsync();
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (IsMonitoring == true)
            {
                base.RaiseTagRead(e.UserState.ToString());
                foreach (var listener in TagListeners)
                {
                    listener.TagRead(new RFIDTag
                    {
                        RFIDTagType = RFIDTag.TagType.Long,
                        Tag = e.UserState.ToString()
                    });
                }
            }
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] EPCBuffer = new byte[4096];
            int EPCBufferLen = 0;
            int numberOfTags = 0;

            ContinueReading = true;

            while (ContinueReading)
            {
                var fCmdRet = StaticClassReaderB.Inventory_G2(ref _ComAddr, 0, 0, 0, EPCBuffer, ref EPCBufferLen, ref numberOfTags, _PortHandle);
                if ((fCmdRet == 1) | (fCmdRet == 2) | (fCmdRet == 3) | (fCmdRet == 4) | (fCmdRet == 0xFB))
                {
                    if (EPCBufferLen > 0)
                    {
                        int cnt = 0;
                        for (int i = 0; ;)
                        {
                            if (cnt == numberOfTags)
                                break;
                            var tag = BitConverter.ToString(EPCBuffer, i + 1, EPCBuffer[i]);
                            (sender as BackgroundWorker)?.ReportProgress(0, tag);
                            i = i + EPCBuffer[i] + 1;
                            cnt++;
                        }
                    }
                }

                Thread.Sleep(250);
            }
        }

        public override void Dispose()
        {
            ContinueReading = false;
            StaticClassReaderB.CloseComPort();
        }

        public override void OnReaderStopped()
        {
            base.OnReaderStopped();
            ContinueReading = false;
        }
    }
}
