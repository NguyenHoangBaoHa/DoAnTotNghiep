﻿using Backend.Repository.Account;
using Backend.Repository.Pitch;
using Backend.Repository.PitchType;
using Backend.Repository.Staff;

namespace Backend.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }

        IStaffRepository Staffs { get; }

        IPitchRepository Pitches { get; }

        IPitchTypeRepository PitchesType { get; }

        Task<int> CompleteAsync();
    }
}
