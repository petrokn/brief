#!/bin/bash

serialnumber=$1

for sysdevpath in $(find /sys/bus/usb/devices/usb*/ -name dev); do
    (
        syspath="${sysdevpath%/dev}"
        devname="$(udevadm info -q name -p $syspath)"
        [[ "$devname" == "bus/"* ]] && continue
        eval "$(udevadm info -q property --export -p $syspath)"
        [[ -z "$ID_SERIAL" ]] && continue
        if [[ "$ID_SERIAL" = *"$serialnumber"* ]]; then
            for mountpath in $(df /dev/"$devname" --output=target | tail -n1); do
            (
                if [[ "$mountpath" != "/dev" ]]; then
                    echo "$mountpath"
                fi
            )
            done
        fi
    )
done