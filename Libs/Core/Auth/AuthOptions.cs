using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ExampleCore.AuthOptions;

public class AuthOptions
{
    public const string ISSUER = "CyberLab"; 
    public const string AUDIENCE = "UDVUser"; 
    const string KEY = @"
                       ░░░░░░░░░░░░░░░░░░░░
                       ░░░░░ЗАПУСКАЕМ░░░░░░░
                       ░ГУСЯ░▄▀▀▀▄░РАБОТЯГИ░░
                       ▄███▀░◐░░░▌░░░░░░░░░
                       ░░░░▌░░░░░▐░░░░░░░░░
                       ░░░░▐░░░░░▐░░░░░░░░░
                       ░░░░▌░░░░░▐▄▄░░░░░░░
                       ░░░░▌░░░░▄▀▒▒▀▀▀▀▄
                       ░░░▐░░░░▐▒▒▒▒▒▒▒▒▀▀▄
                       ░░░▐░░░░▐▄▒▒▒▒▒▒▒▒▒▒▀▄
                       ░░░░▀▄░░░░▀▄▒▒▒▒▒▒▒▒▒▒▀▄
                       ░░░░░░▀▄▄▄▄▄█▄▄▄▄▄▄▄▄▄▄▄▀▄
                       ░░░░░░░░░░░▌▌░▌▌░░░░░
                       ░░░░░░░░░░░▌▌░▌▌░░░░░
                       ░░░░░░░░░▄▄▌▌▄▌▌░░░░░"; 

    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new (Encoding.UTF8.GetBytes(KEY));
}