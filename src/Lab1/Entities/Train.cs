using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public class Train
{
    public double Mass { get; }

    public Speed Speed { get; private set; }

    public double Acceleration { get; private set; }

    public Force MaxForce { get; }

    public Time Precision { get; }

    public Train(double mass, Force maxForce, Time precision)
    {
        if (mass <= 0) throw new ArgumentException("Mass must be positive");
        ArgumentNullException.ThrowIfNull(maxForce);
        ArgumentNullException.ThrowIfNull(precision);

        Mass = mass;
        MaxForce = maxForce;
        Precision = precision;
        Speed = Speed.Zero;
        Acceleration = 0;
    }

    public bool ApplyForce(Force force)
    {
        ArgumentNullException.ThrowIfNull(force);
        if (Math.Abs(force.Newtons) > MaxForce.Newtons) return false;

        Acceleration = force.Newtons / Mass;
        return true;
    }

    public TrainOperationResult PassDistance(Distance distance)
    {
        ArgumentNullException.ThrowIfNull(distance);

        if (Math.Abs(Speed.MetersPerSecond) < 0.001 && Acceleration <= 0)
            return new TrainOperationResult.Failure();

        double remainingDistance = distance.Meters;
        double totalTime = 0.0;
        Speed currentSpeed = Speed;

        while (remainingDistance > 0)
        {
            var resultantSpeed = new Speed(currentSpeed.MetersPerSecond + (Acceleration * Precision.Seconds));

            if (resultantSpeed.MetersPerSecond <= 0)
                return new TrainOperationResult.Failure();

            double distanceCovered = resultantSpeed.MetersPerSecond * Precision.Seconds;

            if (distanceCovered > remainingDistance)
            {
                double exactTime = remainingDistance / resultantSpeed.MetersPerSecond;

                totalTime += exactTime;
                remainingDistance = 0;
                currentSpeed = resultantSpeed;
            }
            else
            {
                remainingDistance -= distanceCovered;
                totalTime += Precision.Seconds;
                currentSpeed = resultantSpeed;
            }
        }

        Speed = currentSpeed;

        return new TrainOperationResult.Success(new Time(totalTime));
    }

    // braking and acceleration methods
    public TrainOperationResult AccelerateToSpeed(Speed targetSpeed)
    {
        ArgumentNullException.ThrowIfNull(targetSpeed);

        if (targetSpeed.MetersPerSecond <= 0)
            return new TrainOperationResult.Failure();

        if (Math.Abs(Speed.MetersPerSecond - targetSpeed.MetersPerSecond) < 0.001)
            return new TrainOperationResult.Success(Time.Zero);

        Time timeToReach = CalculateTimeToReachSpeed(targetSpeed);
        if (timeToReach.Seconds < 0)
            return new TrainOperationResult.Failure();

        var requiredForce = new Force(Math.Abs(MaxForce.Newtons * 0.7));

        if (!ApplyForce(requiredForce))
            return new TrainOperationResult.Failure();

        Distance accelerationDistance = CalculateAccelerationDistance(targetSpeed);
        Time simulationTime = CalculateTimeToReachSpeed(targetSpeed);

        Speed = targetSpeed;

        return new TrainOperationResult.Success(simulationTime);
    }

    public TrainOperationResult BrakeToStop()
    {
        if (Math.Abs(Speed.MetersPerSecond) < 0.001)
            return new TrainOperationResult.Success(Time.Zero);

        Time timeToStop = CalculateTimeToStop();
        if (timeToStop.Seconds < 0)
            return new TrainOperationResult.Failure();

        double requiredAcceleration = -Speed.MetersPerSecond / timeToStop.Seconds;

        var requiredForce = new Force(-Math.Abs(MaxForce.Newtons * 0.8));

        if (!ApplyForce(requiredForce))
            return new TrainOperationResult.Failure();

        Distance brakingDistance = CalculateBrakingDistance();
        Time brakingTime = CalculateTimeToStop();

        Speed = Speed.Zero;
        Acceleration = 0;

        return new TrainOperationResult.Success(brakingTime);
    }

    // calculation methods
    public Distance CalculateBrakingDistance()
    {
        if (Acceleration >= 0 || Math.Abs(Speed.MetersPerSecond) < 0.001) return Distance.Zero;

        // s = (v² - v0²) / (2a), но для остановки v=0 => s = -v0² / (2a)
        return new Distance(-Math.Pow(Speed.MetersPerSecond, 2) / (2 * Acceleration));
    }

    public Distance CalculateAccelerationDistance(Speed targetSpeed)
    {
        ArgumentNullException.ThrowIfNull(targetSpeed);

        if (targetSpeed.MetersPerSecond <= Speed.MetersPerSecond || Acceleration <= 0)
            return Distance.Zero;

        double accelerationDistance = (Math.Pow(targetSpeed.MetersPerSecond, 2) - Math.Pow(Speed.MetersPerSecond, 2)) / (2 * Acceleration);
        return new Distance(accelerationDistance);
    }

    public Time CalculateTimeToReachSpeed(Speed targetSpeed)
    {
        ArgumentNullException.ThrowIfNull(targetSpeed);

        if (Math.Abs(Speed.MetersPerSecond - targetSpeed.MetersPerSecond) < 0.001 || Acceleration <= 0)
            return Time.Zero;

        double timeSeconds = (targetSpeed.MetersPerSecond - Speed.MetersPerSecond) / Acceleration;
        return new Time(Math.Max(0, timeSeconds));
    }

    public Time CalculateTimeToStop()
    {
        if (Math.Abs(Speed.MetersPerSecond) < 0.001 || Acceleration >= 0)
            return Time.Zero;

        double deceleration;
        if (Acceleration >= 0)
        {
            deceleration = Math.Abs(MaxForce.Newtons * 0.8 / Mass);
        }
        else
        {
            deceleration = Math.Abs(Acceleration);
        }

        if (deceleration <= 0)
            return Time.Zero;

        double timeSeconds = Speed.MetersPerSecond / deceleration;
        return new Time(timeSeconds);
    }
}
