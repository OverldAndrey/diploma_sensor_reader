using System;
using Microsoft.ML.Data;

namespace TestSensors
{
    public interface ISensorFrame
    {
        public Single[] readings { get; set; }
        public string Label { get; set; }
    }
    
    public class FourSensorsSingleFrame : ISensorFrame
    {
        [LoadColumn(0, 4 * 100 - 1)]
        [VectorType(4 * 100)]
        public Single[] readings { get; set; }

        [LoadColumn(4 * 100)]
        public string Label { get; set; }
    }
    
    public class FourSensorsHalfFrame : ISensorFrame
    {
        [LoadColumn(0, 4 * 50 - 1)]
        [VectorType(4 * 50)]
        public Single[] readings { get; set; }

        [LoadColumn(4 * 50)]
        public string Label { get; set; }
    }
    
    public class FourSensorsDoubleFrame : ISensorFrame
    {
        [LoadColumn(0, 4 * 200 - 1)]
        [VectorType(4 * 200)]
        public Single[] readings { get; set; }

        [LoadColumn(4 * 200)]
        public string Label { get; set; }
    }
    
    public class TwoSensorsSingleFrame : ISensorFrame
    {
        [LoadColumn(0, 2 * 100 - 1)]
        [VectorType(2 * 100)]
        public Single[] readings { get; set; }

        [LoadColumn(2 * 100)]
        public string Label { get; set; }
    }
    
    public class TwoSensorsHalfFrame : ISensorFrame
    {
        [LoadColumn(0, 2 * 50 - 1)]
        [VectorType(2 * 50)]
        public Single[] readings { get; set; }

        [LoadColumn(2 * 50)]
        public string Label { get; set; }
    }
    
    public class TwoSensorsDoubleFrame : ISensorFrame
    {
        [LoadColumn(0, 2 * 200 - 1)]
        [VectorType(2 * 200)]
        public Single[] readings { get; set; }

        [LoadColumn(2 * 200)]
        public string Label { get; set; }
    }
}