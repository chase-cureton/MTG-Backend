using MTGLambda.MTGLambda.DataRepository.Dao;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.DataRepository
{
    public class DaoFactory
    {
        #region Constructors
        public DaoFactory()
        {
            Setup(new DaoContext());
        }
        #endregion

        #region Fields
        private Lazy<DaoCard> _daoCard;
        private Lazy<DaoDeckCard> _daoDeckCard;
        private Lazy<DaoDeckStats> _daoDeckStats;
        #endregion

        #region Properties
        public DaoCard Cards => _daoCard.Value;
        public DaoDeckCard DeckCards => _daoDeckCard.Value;
        public DaoDeckStats DeckStats => _daoDeckStats.Value;
        #endregion

        private void Setup(DaoContext daoContext)
        {
            _daoCard = new Lazy<DaoCard>(() => new DaoCard(daoContext));
            _daoDeckCard = new Lazy<DaoDeckCard>(() => new DaoDeckCard(daoContext));
            _daoDeckStats = new Lazy<DaoDeckStats>(() => new DaoDeckStats(daoContext));
        }
    }
}
