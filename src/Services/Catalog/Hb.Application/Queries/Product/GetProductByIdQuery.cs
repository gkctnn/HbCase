﻿using MediatR;
using Hb.Application.Responses;

namespace Hb.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductResponse>
    {
        public string Id { get; set; }

        public GetProductByIdQuery(string id)
        {
            Id = id;
        }
    }
}
