using System;
using System.Collections.Generic;
using AutoMapper;
using Digipolis.Web.Api;
using Digipolis.Web.SampleApi.Data;
using Digipolis.Web.SampleApi.Data.Entiteiten;
using Digipolis.Web.SampleApi.Models;

namespace Digipolis.Web.SampleApi.Logic
{
    public class ValueLogic : IValueLogic
    {
        private readonly IMapper _mapper;
        private readonly IValueRepository _valueRepository;

        public ValueLogic(IMapper mapper, IValueRepository valueRepository)
        {
            _mapper = mapper;
            _valueRepository = valueRepository;
        }

        public IEnumerable<ValueDto> GetAll(PageOptions queryOptions, out int total)
        {
            var result = _valueRepository.GetAll(queryOptions, out total);
            return _mapper.Map<IEnumerable<Value>, IEnumerable<ValueDto>>(result);
        }

        public ValueDto GetById(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException();
            return _mapper.Map<Value, ValueDto>(_valueRepository.GetById(id));
        }

        public ValueDto Add(ValueDto value)
        {
            if (value == null) throw new ArgumentNullException();
            var entity = _mapper.Map<ValueDto, Value>(value);
            value = _mapper.Map<Value, ValueDto>(_valueRepository.Add(entity));
            return value;
        }

        public ValueDto Update(int id, ValueDto value)
        {
            if (id < 0) throw new ArgumentOutOfRangeException();
            if (value == null) throw new ArgumentNullException();

            var entity = _mapper.Map<ValueDto, Value>(value);
            value = _mapper.Map<Value, ValueDto>(_valueRepository.Update(id, entity));
            return value;
        }

        public void Delete(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException();
            _valueRepository.Delete(id);
        }
    }
}