using System;
using AutoMapper;

namespace ItemsAdministration.Common.Shared.Extensions;

public static class ProfileExtensions
{
    public static void ConstructUsingWithIgnoreAll<TSource, TDestination>(this Profile profile, Func<TSource, ResolutionContext, TDestination> ctor)
    {
        profile.CreateMap<TSource, TDestination>().ConstructUsing(ctor).ForAllMembers(delegate (IMemberConfigurationExpression<TSource, TDestination, object> opt)
        {
            opt.Ignore();
        });
    }

}