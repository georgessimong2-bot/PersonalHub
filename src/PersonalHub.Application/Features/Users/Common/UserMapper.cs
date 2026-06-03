using PersonalHub.Application.Features.Notes.Common;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Users.Common;



public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Email = user.Email
        };
    }

    public static List<NoteDto> ToDto(this IEnumerable<Note> notes)
    {
        return notes.Select(x => x.ToDto()).ToList();
    }
}
