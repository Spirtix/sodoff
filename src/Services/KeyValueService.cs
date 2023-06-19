using sodoff.Model;
using sodoff.Schema;

namespace sodoff.Services;
public class KeyValueService {

    private readonly DBContext ctx;

    public KeyValueService(DBContext ctx) {
        this.ctx = ctx;
    }

    public Model.PairData? GetPairData(Session session, int pairId) {
        Model.PairData? pair = null;
        if (session.VikingId != null)
            pair = ctx.PairData.FirstOrDefault(e => e.PairId == pairId && e.VikingId == session.VikingId);
        else if (session.UserId != null)
            pair = ctx.PairData.FirstOrDefault(e => e.PairId == pairId && e.UserId == session.UserId);

        return pair;
    }

    public bool SetPairData(Session session, int pairId, Schema.PairData schemaData) {
        // Get the pair
        Model.PairData? pair = GetPairData(session, pairId);

        // Create the pair if it doesn't exist
        bool exists = true;
        if (pair is null) {
            pair = new Model.PairData {
                PairId = pairId,
                UserId = session.UserId,
                VikingId = session.VikingId,
                Pairs = new List<Model.Pair>()
            };
            exists = false;
        }

        // Update or create the key-value pairs
        foreach (var p in schemaData.Pairs) {
            if (string.IsNullOrEmpty(p.PairValue))
                return false; // NOTE: The real server doesn't update anything if one value is empty
            var pairItem = pair.Pairs.FirstOrDefault(e => e.Key == p.PairKey);
            if (pairItem != null)
                pairItem.Value = p.PairValue;
            else {
                pairItem = new Model.Pair {
                    Key = p.PairKey,
                    Value = p.PairValue
                };
                pair.Pairs.Add(pairItem);
            }
        }

        if (exists)
            ctx.PairData.Update(pair);
        else
            ctx.PairData.Add(pair);
        ctx.SaveChanges();

        return true;
    }

    public Schema.PairData? ModelToSchema(Model.PairData? model) {
        if (model is null) return null;

        // Convert to schema class
        List<Schema.Pair> pairs = new List<Schema.Pair>();
        foreach (var p in model.Pairs) {
            pairs.Add(new Schema.Pair {
                PairKey = p.Key,
                PairValue = p.Value
            });
        }

        return new Schema.PairData { Pairs = pairs.ToArray() };
    } 

}
