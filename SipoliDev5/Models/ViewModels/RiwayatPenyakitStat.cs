using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SipoliDev5.Models.ViewModels
{
    public class RiwayatPenyakitStat
    {
        public RiwayatPenyakitStat(int no, int PenyakitID, int PasienCount){
            this.no=no;
            this.PenyakitID=PenyakitID;
            this.PasienCount=PasienCount;
        }

        public int no{get;set;}
        public int PenyakitID {get;set;}
        public int PasienCount{get;set;}
    }
}