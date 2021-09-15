using System;
using System.Collections.Generic;
using System.Text;

namespace WorkReportApp.DAL
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
