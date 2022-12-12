
--instructions at the bottom

TimeTween = {}

local lighting = game:GetService("Lighting")

--converts time String to array
local function TimeStringToArray(timeString)
	local timeArray = timeString:split(":")
	for i,v in next, timeArray do v = tonumber(v) end
	return timeArray
end

--converts time array to seconds
local function ArrayToSeconds(timeArray)
	return (timeArray[1]*60*60) + (timeArray[2]*60) + timeArray[3]
end

--adds a given amount to the time at once
local function AddOnce(toAdd)
	--current time to array, dump all the given seconds to the array seconds
    local times = TimeStringToArray(lighting.TimeOfDay)
    times[3] += toAdd
    
	--turns seconds to hours, minutes, seconds
    repeat
        if times[3] >= 60 then
            times[3] -= 60
            times[2] += 1
            
            if times[2] >= 60 then
                times[2] = 0
                times[1] += 1
                
                if times[1] >= 24 then
                    times[1] = 0
                end
            end
        end
    until times[3] < 60

	--array to time string and sets the time to it
    lighting.TimeOfDay = table.concat(times, ":")
end

local function TimeUpdater(howMuch, howLong, updateSpeed)

	updateSpeed = (updateSpeed < 30 and updateSpeed > 0 and updateSpeed) or 30 --max 30, min >0, default: 30
	howLong = howLong or 1 --defaults to 1

	local waitTime = 1 / updateSpeed --updates per second -> time between updates
	
	--if time is 0, tween is instant
	local timeChangePerMove
	if howLong = 0 then
		timeChangePerMove = howMuch
	else
		timeChangePerMove = howMuch / howLong / updateSpeed --how much time is added each update
	end

	local timeStack = 0 --stacking time to stop problems with decimal numbers
	while howMuch > 0 do
		timeStack += timeChangePerMove
		if timeStack >= 1 then
			local adding = math.floor(timeStack) --leave decimals for the next update
			TimeTween:AddOnce(adding)
			timeStack -= adding
		end
		task.wait(waitTime)
		howMuch -= timeChangePerMove
	end
end

function TimeTween:Add(howMuch, howLong, updateSpeed)
	--convert time String to seconds
	timeToAdd = ArrayToSeconds(TimeStringToArray(howMuch))
	--send to time updater
	TimeUpdater(timeToAdd, howLong, updateSpeed)
end

function TimeTween:Set(targetTime, howLong, updateSpeed)

	--split the times and add to a table as numbers
	local toSet = TimeStringToArray(targetTime)
	local times = TimeStringToArray(lighting.TimeOfDay)
	
	local timeToAdd = 0
	--if the old time is bigger or same as the new time then go to the next day
	if ArrayToSeconds(times) >= ArrayToSeconds(toSet)) then 
		timeToAdd = ((24*60*60) - ArrayToSeconds(times)) + ArrayToSeconds(toSet)
		--seconds left for the next day + seconds to the target time from the start of the day
	else
		timeToAdd = ArrayToSeconds(toSet) - ArrayToSeconds(times)
		--time difference between the two
	end
	TimeTween:TimeUpdater(timeToAdd, howLong, updateSpeed)
end

--how to use:

TimeTween:Add("12:00:00", 10) --tweens time forward the given amount in the given span
--first argument: string, amount of time to add
--second argument: number, tween length in seconds
--third agument: number, time updates per second (optional)

TimeTween:Set("12:30:00", 10) --tweens time to the given time in the given span
--first argument: string, time where to tween
--second argument: number, tween length in seconds
--third agument: number, time updates per second (optional)

