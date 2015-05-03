using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GoSimple.Logging.Test
{
    public abstract class GivenWhenThen
    {

        [SetUp]
        public void Inititialize()
        {
            this.Given();
            this.When();
        }

        public virtual void Given(){}

        public virtual void When(){}

    }
}
