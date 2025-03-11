﻿using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IClassRepository
    {
        List<Class> GetAllClass();
        Class? GetClassById(int id);
    }
}
