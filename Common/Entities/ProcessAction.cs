using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    /// <summary>
    /// Action being performed on an entity.
    /// </summary>
    public enum ProcessAction
    {
        /// <summary>
        /// Entity is being created.
        /// </summary>
        Create,

        /// <summary>
        /// Entity is being read.
        /// </summary>
        Read,

        /// <summary>
        /// Entity is being updated.
        /// </summary>
        Update,

        /// <summary>
        /// Entity is being deleted.
        /// </summary>
        Delete,

        /// <summary>
        /// Entity is being reinstated.
        /// </summary>
        Reinstate,

        /// <summary>
        /// Entity is being relocated.
        /// </summary>
        Relocate
    }
}
