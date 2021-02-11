﻿using DSU21_5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSU21_5.Data
{
    public class MemberRepository : IMemberRepository
    {
        public IImageDbContext db { get; set; }
        public MemberRepository(IImageDbContext context)
        {
            db = context;
        }

        public async Task<Member> AddMember(Member member)
        {
            db.Add(member);
            await db.SaveChangesAsync();
            return member;
        }
        public async Task<Member> GetMember(string Id)
        {
            Member member = db.Members.Where(x => x.MemberId == Id).FirstOrDefault();
            await db.SaveChangesAsync();
            return member;
        }

        public async Task<List<Member>> GetAllMembers()
        {
            List<Member> members = new List<Member>();

            List<String> Ids = db
            .Members
            .Select(u => u.MemberId)
            .ToList();

            foreach (string id in Ids)
            {
                Member member = await GetMember(id);
                members.Add(member);
            }

            return members;
        }

        public async Task<Member> UpdateBio(string Id, string bio)
        {
            var update = db.Members.Where(x => x.MemberId == Id).FirstOrDefault();
            update.Bio = bio;
            db.Update(update);

            await db.SaveChangesAsync();
            return update;
        }

        public async Task<Member> UpdateFacebook(string Id, string facebook)
        {
            var update = db.Members.Where(x => x.MemberId == Id).FirstOrDefault();
            update.Facebook = facebook;
            db.Update(update);

            await db.SaveChangesAsync();
            return update;
        }

        public async Task<Member> UpdateTwitter(string Id, string twitter)
        {
            var update = db.Members.Where(x => x.MemberId == Id).FirstOrDefault();
            update.Twitter = twitter;
            db.Update(update);

            await db.SaveChangesAsync();
            return update;
        }

        public async Task<Member> UpdateInstagram(string Id, string instagram)
        {
            var update = db.Members.Where(x => x.MemberId == Id).FirstOrDefault();
            update.Instagram = instagram;
            db.Update(update);

            await db.SaveChangesAsync();
            return update;
        }
    }
}
