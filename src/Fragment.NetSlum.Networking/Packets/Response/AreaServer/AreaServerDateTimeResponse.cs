using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.OnlineEvents;
using System;
using System.Buffers.Binary;
using System.Linq;

namespace Fragment.NetSlum.Networking.Packets.Response.AreaServer
{
    /// <summary>
    /// Handles the response sent to the AreaServer containing the current date or an event-specific date.
    /// </summary>
    public class AreaServerDateTimeResponse : BaseResponse
    {
        /// <summary>
        /// Builds the response packet to send the date information to the AreaServer.
        /// </summary>
        /// <returns>A FragmentMessage containing the appropriate date.</returns>
        public override FragmentMessage Build()
        {
            // Initialize the buffer to write the date information (8 bytes total).
            var writer = new MemoryWriter(8);

            // Write a placeholder value (0) to the first 4 bytes of the buffer.
            writer.Write((uint)0);

            // Get the current date and time in UTC.
            var currentDate = DateTime.UtcNow;

            // Check if the current date matches any event in the predefined event list.
            var matchingEvent = EventData.Events.FirstOrDefault(e =>
                e.StartDate.Date <= currentDate.Date && 
                (e.EndDate?.Date ?? e.StartDate.Date) >= currentDate.Date);

            // If an event is found, use its date; otherwise, use the current date.
            var dateToSend = matchingEvent?.StartDate ?? currentDate;

            // Convert the selected date to a Unix timestamp and write it to the buffer.
            writer.Write((uint)dateToSend.ToEpoch());

            // Log the result for debugging purposes.
            Console.WriteLine(matchingEvent != null
                ? $"[AreaServerDateTimeResponse] Sending event date: {matchingEvent.Name} ({dateToSend})"
                : $"[AreaServerDateTimeResponse] Sending current date: {dateToSend}");

            // Return the constructed FragmentMessage with the date information.
            return new FragmentMessage
            {
                MessageType = MessageType.Data,
                DataPacketType = OpCodes.Data_AreaServerDateTimeSuccess,
                Data = writer.Buffer,
            };
        }
    }
}
