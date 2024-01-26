﻿using CRS.ADMIN.REPOSITORY.ReviewAndRatingsManagement;
using CRS.ADMIN.SHARED;
using CRS.ADMIN.SHARED.PaginationManagement;
using CRS.ADMIN.SHARED.ReviewAndRatingsManagement;
using System.Collections.Generic;

namespace CRS.ADMIN.BUSINESS.ReviewAndRatingsManagement
{
    public class ReviewAndRatingsBusiness : IReviewAndRatingsBusiness
    {
        private readonly IReviewAndRatingsRepository _repository;
        public ReviewAndRatingsBusiness(ReviewAndRatingsRepository repository) => _repository = repository;

        public CommonDbResponse DeleteReview(string reviewId, string actionUser, string actionIp)
        {
            return _repository.DeleteReview(reviewId, actionUser, actionIp);
        }
        public List<ReviewCommon> GetReviews(PaginationFilterCommon Request, string reviewId = "")
        {
            return _repository.GetReviews(Request, reviewId);
        }
    }
}