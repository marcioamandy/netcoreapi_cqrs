﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.ViewModels
{
    public class FornecedorFilterViewModel : BaseFilterViewModel
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca")]
        public string Search { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public FornecedorFilterViewModel()
		{
		}
	}
}
