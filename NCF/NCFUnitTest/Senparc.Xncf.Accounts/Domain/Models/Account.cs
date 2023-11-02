using Senparc.Ncf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senparc.Xncf.Accounts.Domain.Models
{
    [Serializable]
    [Table("Accounts")]
    public partial class Account : EntityBase<int>
    {
        private Account()
        {
            PointsLogs = new List<PointsLog>();
        }

        public string UserName { get; private set; }
        //[MaxLength(100)]
        public string Password { get; private set; }
        //[MaxLength(100)]
        public string PasswordSalt { get; private set; }
        //[Required,MaxLength(50)]
        public string NickName { get; private set; }
        public string RealName { get; private set; }
        public string Phone { get; private set; }
        public bool? PhoneChecked { get; private set; }

        public string Email { get; private set; }
        public bool? EmailChecked { get; private set; }
        public string PicUrl { get; private set; }
        public string HeadImgUrl { get; private set; }
        public decimal Package { get; private set; }
        public decimal Balance { get; private set; }
        public decimal LockMoney { get; private set; }
        public byte Sex { get; private set; }
        public string QQ { get; private set; }
        [MaxLength(30)]
        public string Country { get; private set; }
        public string Province { get; private set; }
        [MaxLength(30)]
        public string City { get; private set; }
        public string District { get; private set; }
        public string Address { get; private set; }
        public string Note { get; private set; }
        public DateTime ThisLoginTime { get; private set; }
        public string ThisLoginIp { get; private set; }
        public DateTime LastLoginTime { get; private set; }
        public string LastLoginIP { get; private set; }
        public decimal Points { get; private set; }
        public DateTime? LastWeixinSignInTime { get; private set; }
        public int WeixinSignTimes { get; private set; }
        public string WeixinUnionId { get; private set; }

        public string WeixinOpenId { get; private set; }

        /// <summary>
        /// 是否被锁定（无法登陆） TODO：暂未添加到DTO中
        /// </summary>
        public bool? Locked { get; private set; }

        public ICollection<PointsLog> PointsLogs { get; private set; }
        public ICollection<AccountPayLog> AccountPayLogs { get; private set; }

        public Account(string userName, string password, string passwordSalt, string nickName, string realName, string phone,
            bool? phoneChecked, string email, bool? emailChecked, string picUrl, string headImgUrl, decimal package, decimal balance, 
            decimal lockMoney, byte sex, string qq, string country, string province, string city, string district, string address, 
            string note, DateTime thisLoginTime, string thisLoginIp, DateTime lastLoginTime, string lastLoginIP, decimal points, 
            DateTime? lastWeixinSignInTime, int weixinSignTimes, string weixinUnionId, string weixinOpenId, bool? locked)
        {
            UserName = userName;
            Password = password;
            PasswordSalt = passwordSalt;
            NickName = nickName;
            RealName = realName;
            Phone = phone;
            PhoneChecked = phoneChecked;
            Email = email;
            EmailChecked = emailChecked;
            PicUrl = picUrl;
            HeadImgUrl = headImgUrl;
            Package = package;
            Balance = balance;
            LockMoney = lockMoney;
            Sex = sex;
            QQ = qq;
            Country = country;
            Province = province;
            City = city;
            District = district;
            Address = address;
            Note = note;
            ThisLoginTime = thisLoginTime;
            ThisLoginIp = thisLoginIp;
            LastLoginTime = lastLoginTime;
            LastLoginIP = lastLoginIP;
            Points = points;
            LastWeixinSignInTime = lastWeixinSignInTime;
            WeixinSignTimes = weixinSignTimes;
            WeixinUnionId = weixinUnionId;
            WeixinOpenId = weixinOpenId;
            Locked = locked;

            PointsLogs = new List<PointsLog>();
        }
    }
}
