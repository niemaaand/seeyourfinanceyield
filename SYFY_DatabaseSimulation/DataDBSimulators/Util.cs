using SYFY_Domain.model;
using System;
using System.Collections.Generic;

namespace SYFY_Plugin_DatabaseSimulation.DataDBSimulators
{
    internal class Util
    {
        internal static Guid GenerateNewGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="savedData"></param>
        /// <param name="keys"></param>
        /// <returns>true: save newly, false: update</returns>
        /// <exception cref="InvalidOperationException"></exception>
        static internal (bool, Guid) ShouldDataBeSaved(DeleteableData data, DeleteableData savedData, ICollection<Guid> keys)
        {
            if (savedData == null)
            {
                // save newly

                Guid id;

                do
                {
                    id = GenerateNewGuid();
                } while (keys.Contains(id));

                return (true, id);
            }
            else if (savedData.Deleted || data.Deleted)
            {
                throw new InvalidOperationException("Data is deleted and can therefore not be changed.");
            }
            else
            {
                return (false, Guid.Empty);
            }

        }
    }
}
