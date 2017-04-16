using System;
using System.Security.Cryptography.X509Certificates;
using BarsGroup.CodeGuard.Internals;

namespace BarsGroup.CodeGuard.Validators
{
    public static class NullableValidatorExtensions
    {
        /// <summary>
        ///     Is argument instance of type
        /// </summary>
        /// <returns></returns>
        [GuardRedirectAttibute("IsNotNull")]
        public static ArgBase<T?> IsNotNull<T>(this ArgBase<T?> arg) where T : struct
        {
            throw new InvalidOperationException();
        }

        public static void IsNotNullValidate<T>(T? value, string param) where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException();
            }
                //(value, param).ThrowArgumentNull();
        }
    }
}