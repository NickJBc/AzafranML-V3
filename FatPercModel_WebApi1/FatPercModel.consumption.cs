﻿// This file was auto-generated by ML.NET Model Builder.

using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.ML.Transforms.TimeSeries;

public partial class FatPercModel
{
    /// <summary>
    /// model input class for FatPercModel.
    /// </summary>
    #region model input class
    public class ModelInput
    {
        [LoadColumn(1)]
        [ColumnName(@"Fat Percentage")]
        public float Fat_Percentage { get; set; }

    }

    #endregion

    /// <summary>
    /// model output class for FatPercModel.
    /// </summary>
    #region model output class
    public class ModelOutput
    {
        [ColumnName(@"Fat Percentage")]
        public float[] Fat_Percentage { get; set; }

        [ColumnName(@"Fat Percentage_LB")]
        public float[] Fat_Percentage_LB { get; set; }

        [ColumnName(@"Fat Percentage_UB")]
        public float[] Fat_Percentage_UB { get; set; }

    }

    #endregion

    private static string MLNetModelPath = Path.GetFullPath(@"FatPercModel.mlnet");

    public static readonly Lazy<TimeSeriesPredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<TimeSeriesPredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

    /// <summary>
    /// Use this method to predict on <see cref="ModelInput"/>.
    /// </summary>
    /// <param name="input">model input.</param>
    /// <returns><seealso cref=" ModelOutput"/></returns>
    public static ModelOutput Predict(ModelInput? input = null, int? horizon = null)
    {
        var predEngine = PredictEngine.Value;
        return predEngine.Predict(input, horizon);
    }

    private static TimeSeriesPredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
    {
        var mlContext = new MLContext();
        ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var schema);
        return mlModel.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);
    }
}

