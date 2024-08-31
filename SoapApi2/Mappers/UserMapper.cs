using SoapApi2.Dtos;
using SoapApi2.Infrastructure.Entities;
using SoapApi2.Models;

namespace SoapApi2.Mappers;

public static class UserMapper {
    public static UserModel ToModel(this UserEntity user) {
        if (user is null) {
            return null;
        }

        if (user is null) {
            return null;
        }

        return new UserModel {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate= user.Birthday
        };
    }

    public static UserResponseDto ToDto(this UserModel user) {
        return new UserResponseDto {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate
        };
    }
}