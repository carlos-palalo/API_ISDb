﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace API_ISDb.Examples
{
    public class ESerie
    {
        public string Titulo { get; set; }
        public string Poster { get; set; }
        public string Year { get; set; }
        public string Sinopsis { get; set; }
        public string Trailer { get; set; }
    }
}