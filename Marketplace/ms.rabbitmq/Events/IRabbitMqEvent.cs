using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ms.rabbitmq.Events;

public interface IRabbitMqEvent
{
    string Serialize();
}
