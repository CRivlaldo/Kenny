module CRC16
    let CRCLength = 2

    let GetCRC16(buf : byte[], len) =
        let mutable crc = uint16 0xFFFF
        for pos in [0..len-1] do //!!!vladimir: fold???
            crc <- crc ^^^ (uint16 buf.[pos])
            for i in [8..-1..1] do // Loop over each bit
                if ((crc &&& (uint16 0x0001)) <> (uint16 0)) then // If the LSB is set
                    crc <- (crc >>> 1)  // Shift right and XOR 0xA001
                    crc <- (crc ^^^ (uint16 0xA001))
                else crc <- (crc >>> 1) // Else LSB is not set, thus just shift right
        crc

