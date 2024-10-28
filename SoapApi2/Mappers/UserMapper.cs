using SoapApi2.Dtos;
using SoapApi2.Infrastructure.Entities;
using SoapApi2.Models;

namespace SoapApi2.Mappers;

public static class UserMapper {
    public static UserModel ToModel(this UserEntity user) {
        if (user is null) {
            return null;
        }

        return new UserModel {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.Birthday  // Asegúrate que en UserEntity se llame "Birthday"
        };
    }

    public static UserResponseDto ToDto(this UserModel user) {
        return new UserResponseDto {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate  // Cambiado a BirthDate
        };
    }

    public static UserEntity ToEntity(this UserModel user) {
        return new UserEntity {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Birthday = user.BirthDate  // Cambiado a BirthDate
        };
    }

    public static UserModel ToModel(this UserCreateRequestDto user) {
        return new UserModel {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = DateTime.UtcNow  
        };
    }

    public static UserModel ToModel(this UserUpdateRequestDto user) {
        return new UserModel {
            Id = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate  
        };
    }
}
