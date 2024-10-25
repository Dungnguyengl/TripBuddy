using CommonService.Extentions;
using System.Text;

namespace CommonService.Constants
{

    public class ODataParam(int? top = null, int? skip = null, string? orderBy = null, bool count = false)
    {
        private readonly List<string> _selects = [];
        private readonly List<string> _expands = [];
        private readonly OdataFilters _filters = new();

        public string? Select { get => _selects.Count != 0 ? string.Join(", ", _selects) : null; }
        public string? Expand { get => _expands.Count != 0 ? string.Join(", ", _expands) : null; }
        public int? Top { get; set; } = top;
        public int? Skip { get; set; } = skip;
        public string? OrderBy { get; set; } = orderBy;
        public bool Count { get; set; } = count;
        public string? Filter { get => _filters.Filter; }


        public ODataParam AddFilter(Action<OdataFilters>? filters = null)
        {
            filters?.Invoke(_filters);
            return this;
        }

        public ODataParam AddSelect(params string[] selects)
        {
            _selects.AddRange(selects);
            return this;
        }

        public ODataParam AddExpands(params string[] expands)
        {
            _expands.AddRange(expands);
            return this;
        }

        public override string ToString()
        {
            var param = new List<string>();
            if (!Select.IsNullOrEmpty())
                param.Add($"$select={Select}");
            if (!Expand.IsNullOrEmpty())
                param.Add($"$expand={Expand}");
            if (Top.HasValue)
                param.Add($"$top={Top}");
            if (Skip.HasValue)
                param.Add($"$skip={Skip}");
            if (!OrderBy.IsNullOrEmpty())
                param.Add($"$orderBy={OrderBy}");
            if (Count)
                param.Add("$count=true");
            if (!Filter.IsNullOrEmpty())
                param.Add($"$filter={Filter}");
            return param.Count > 0 ? $"?{string.Join('&', param)}" : string.Empty;
        }
    }

    public class ODataFiter(string field, string condition, dynamic value)
    {
        public string Field { get; } = field;
        public string Condition { get; } = condition;
        public dynamic Value { get; } = value;

        public override string ToString()
        {
            if (Condition == ODataConstants.CONTAIN)
                return $"contains({Field}, '{Value}')";
            var value = (Value is string) ? $"'{Value}'" : Value;
            return $"{Field} {Condition} {value}";
        }
    }

    public class OdataFilters()
    {
        private readonly StringBuilder _filter = new();
        public string Filter { get => _filter.ToString(); }

        public OdataFilters WithAnd(ODataFiter condition)
        {
            if (_filter.Length == 0)
            {
                _filter.Append(condition.ToString());
            }
            else
            {
                _filter.Append($" {ODataConstants.AND} {condition.ToString()}");
            }
            return this;
        }

        public OdataFilters WithOr(ODataFiter condition)
        {
            if (_filter.Length == 0)
            {
                _filter.Append(condition.ToString());
            }
            else
            {
                _filter.Append($" {ODataConstants.OR} {condition.ToString()}");
            }
            return this;
        }
    }

    public static class ODataConstants
    {
        public static readonly string EQUAL = "eq";
        public static readonly string NOT_EQUAL = "ne";
        public static readonly string LESS = "lt";
        public static readonly string LESS_OR_EQUAL = "le";
        public static readonly string GREATER = "gt";
        public static readonly string GREATER_OR_EQUAL = "ge";
        public static readonly string CONTAIN = "ct";

        public static readonly string AND = "and";
        public static readonly string OR = "or";
    }
}
