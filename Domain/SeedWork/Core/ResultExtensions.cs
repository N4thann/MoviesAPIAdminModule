namespace Domain.SeedWork.Core
{
    public static class ResultExtensions
    {
        public static BaseResult Combine(this BaseResult firstResult, params BaseResult[] subsequentResults)
        {
            if (firstResult.IsFailure)
                return firstResult;

            foreach (var result in subsequentResults)
                if (result.IsFailure)
                    return result;

            return BaseResult.AsSuccess();
        }
    }
}
