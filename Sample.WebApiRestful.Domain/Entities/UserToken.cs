﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiredDateAccessToken { get; set; }
        [StringLength(50)]
        public string CodeRefreshToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredDateRefreshToken { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
