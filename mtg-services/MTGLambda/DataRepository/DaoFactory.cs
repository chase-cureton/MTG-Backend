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
        private Lazy<DaoDeck> _daoDeck;
        #endregion

        #region Properties
        public DaoCard Cards => _daoCard.Value;
        public DaoDeck Decks => _daoDeck.Value;
        #endregion

        private void Setup(DaoContext daoContext)
        {
            _daoCard = new Lazy<DaoCard>(() => new DaoCard(daoContext));
            _daoDeck = new Lazy<DaoDeck>(() => new DaoDeck(daoContext));
        }
    }
}
