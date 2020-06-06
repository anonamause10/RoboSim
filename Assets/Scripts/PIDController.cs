using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDController
{
    private float m_P;                     // factor for "proportional" control
    private float m_I;                     // factor for "integral" control
    private float m_D;                     // factor for "derivative" control
    private float m_input;                 // sensor input for pid controller
    private float m_maximumOutput = 1.0f;	// |maximum output|
    private float m_minimumOutput = -1.0f;	// |minimum output|
    private float m_maximumInput = 0.0f;	// maximum input - limit setpoint to this
    private float m_minimumInput = 0.0f;	// minimum input - limit setpoint to this
    private bool m_continuous = false;	// do the endpoints wrap around? eg. Absolute encoder
    private bool m_enabled = false;      // is the pid controller enabled
    private float m_prevError = 0.0f;       // the prior sensor input (used to compute velocity)
    private float m_totalError = 0.0f;      // the sum of the errors for use in the integral calc
    private float m_tolerance = 0.05f;      // the percentage error that is considered on target
    private float m_setpoint = 0.0f;
    private float m_error = 0.0f;
    private float m_result = 0.0f;

    /**
     * Allocate a PID object with the given constants for P, I, D
     * @param Kp the proportional coefficient
     * @param Ki the integral coefficient
     * @param Kd the derivative coefficient
     */
    public PIDController(float Kp, float Ki, float Kd)
    {
        m_P = Kp;
        m_I = Ki;
        m_D = Kd;
    }

    /**
     * Read the input, calculate the output accordingly, and write to the output.
     * This should only be called by the PIDTask
     * and is created during initialization.
     */
    private void calculate()
    {
        // If enabled then proceed into controller calculations
        if (m_enabled)
        {
            // Calculate the error signal
            m_error = m_setpoint - m_input;

            // If continuous is set to true allow wrap around
            if (m_continuous)
            {
                if (Mathf.Abs(m_error) > (m_maximumInput - m_minimumInput) / 2)
                {
                    if (m_error > 0)
                        m_error = m_error - m_maximumInput + m_minimumInput;
                    else
                        m_error = m_error + m_maximumInput - m_minimumInput;
                }
            }

            // Integrate the errors as long as the upcoming integrator does
            // not exceed the minimum and maximum output thresholds.

            if ((Mathf.Abs(m_totalError + m_error) * m_I < m_maximumOutput) &&
                    (Mathf.Abs(m_totalError + m_error) * m_I > m_minimumOutput))
                m_totalError += m_error;

            // Perform the primary PID calculation
            m_result = m_P * m_error + m_I * m_totalError + m_D * (m_error - m_prevError);

            // Set the current error to the previous error for the next cycle.
            m_prevError = m_error;

            int sign = (int) Mathf.Round(Mathf.Sign(m_result));    // Record sign of result.

            // Make sure the final result is within bounds. If we constrain the result, we make
            // sure the sign of the constrained result matches the original result sign.
            if (Mathf.Abs(m_result) > m_maximumOutput)
                m_result = m_maximumOutput * sign;
            else if (Mathf.Abs(m_result) < m_minimumOutput)
                m_result = m_minimumOutput * sign;
        }
    }

    /**
     * Set the PID Controller gain parameters.
     * Set the proportional, integral, and differential coefficients.
     * @param p Proportional coefficient
     * @param i Integral coefficient
     * @param d Differential coefficient
     */
    public void setPID(float p, float i, float d)
    {
        m_P = p;
        m_I = i;
        m_D = d;
    }

    /**
     * Get the Proportional coefficient
     * @return proportional coefficient
     */
    public float getP()
    {
        return m_P;
    }

    /**
     * Get the Integral coefficient
     * @return integral coefficient
     */
    public float getI()
    {
        return m_I;
    }

    /**
     * Get the Differential coefficient
     * @return differential coefficient
     */
    public float getD()
    {
        return m_D;
    }

    /**
     * Return the current PID result for the last input set with setInput().
     * This is always centered on zero and constrained the the max and min outs
     * @return the latest calculated output
     */
    public float performPID()
    {
        calculate();
        return m_result;
    }

    /**
     * Return the current PID result for the specified input.
     * @param input The input value to be used to calculate the PID result.
     * This is always centered on zero and constrained the the max and min outs
     * @return the latest calculated output
     */
    public float performPID(float input)
    {
        setInput(input);
        return performPID();
    }

    /**
     *  Set the PID controller to consider the input to be continuous,
     *  Rather then using the max and min in as constraints, it considers them to
     *  be the same point and automatically calculates the shortest route to
     *  the setpoint.
     * @param continuous Set to true turns on continuous, false turns off continuous
     */
    public void setContinuous(bool continuous)
    {
        m_continuous = continuous;
    }

    /**
     *  Set the PID controller to consider the input to be continuous,
     *  Rather then using the max and min in as constraints, it considers them to
     *  be the same point and automatically calculates the shortest route to
     *  the setpoint.
     */
    public void setContinuous()
    {
        this.setContinuous(true);
    }

    /**
     * Sets the maximum and minimum values expected from the input.
     *
     * @param minimumInput the minimum value expected from the input, always positive
     * @param maximumInput the maximum value expected from the output, always positive
     */
    public void setInputRange(float minimumInput, float maximumInput)
    {
        m_minimumInput = Mathf.Abs(minimumInput);
        m_maximumInput = Mathf.Abs(maximumInput);
        setSetpoint(m_setpoint);
    }

    /**
     * Sets the minimum and maximum values to write.
     *
     * @param minimumOutput the minimum value to write to the output, always positive
     * @param maximumOutput the maximum value to write to the output, always positive
     */
    public void setOutputRange(float minimumOutput, float maximumOutput)
    {
        m_minimumOutput = Mathf.Abs(minimumOutput);
        m_maximumOutput = Mathf.Abs(maximumOutput);
    }

    /**
     * Set the setpoint for the PIDController
     * @param setpoint the desired setpoint
     */
    public void setSetpoint(float setpoint)
    {
        int     sign = 1;

        if (m_maximumInput > m_minimumInput)
        {
            if (setpoint < 0) sign = -1;

            if (Mathf.Abs(setpoint) > m_maximumInput)
                m_setpoint = m_maximumInput * sign;
            else if (Mathf.Abs(setpoint) < m_minimumInput)
                m_setpoint = m_minimumInput * sign;
            else
                m_setpoint = setpoint;
        }
        else
            m_setpoint = setpoint;
    }

    /**
     * Returns the current setpoint of the PIDController
     * @return the current setpoint
     */
    public float getSetpoint()
    {
        return m_setpoint;
    }

    /**
     * Set the percentage error which is considered tolerable for use with
     * OnTarget. (Input of 15.0 = 15 percent)
     * @param percent error which is tolerable
     */
    public void setTolerance(float percent)
    {
        m_tolerance = percent;
    }

    /**
     * Return true if the error is within the percentage of the total input range,
     * determined by setTolerance. This assumes that the maximum and minimum input
     * were set using setInputRange.
     * @return true if the error is less than the tolerance
     */
    public bool onTarget()
    {
        return (Mathf.Abs(m_error) < Mathf.Abs(m_tolerance / 100.0f * (m_maximumInput - m_minimumInput)));
    }

    /**
     * Begin running the PIDController
     */
    public void enable()
    {
        m_enabled = true;
    }

    /**
     * Stop running the PIDController.
     */
    public void disable()
    {
        m_enabled = false;
    }

    /**
     * Reset the previous error,, the integral term, and disable the controller.
     */
    public void reset()
    {
        disable();
        m_prevError = 0;
        m_totalError = 0;
        m_result = 0;
    }

    /**
     * Set the input value to be used by the next call to performPID().
     * @param input Input value to the PID calculation.
     */
    public void setInput(float input)
    {
        int     sign = 1;

        if (m_maximumInput > m_minimumInput)
        {
            if (input < 0) sign = -1;

            if (Mathf.Abs(input) > m_maximumInput)
                m_input = m_maximumInput * sign;
            else if (Mathf.Abs(input) < m_minimumInput)
                m_input = m_minimumInput * sign;
            else
                m_input = input;
        }
        else
            m_input = input;
    }
}