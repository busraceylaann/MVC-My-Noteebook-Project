using projemm3.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projemm3.BusinessLayer.Results
{
   public class BusinesLayerResult<T> where T:class
    {
        public List<ErorMessageobj> Errors { get; set; }
        public T Result { get; set; }
        public BusinesLayerResult()
        {
            Errors = new List<ErorMessageobj>();
           

        }
        public void AddError(ErorMessages code,string message)
        {
            Errors.Add(new ErorMessageobj() { Code = code, Message = message });
        }
    }
}
